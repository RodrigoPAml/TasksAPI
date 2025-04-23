using Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Migrations
{
    /// <summary>
    /// Automatic migration applier
    /// </summary>
    public static class MigrationsExecutor
    {
        private static bool IsInitialized = false;

        public static void Execute(InfraContext context)
        {
            if (IsInitialized)
                return;

            IsInitialized = true;
            
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Applying migrations");

                var path = AppDomain.CurrentDomain.BaseDirectory + "\\Migrations\\Sql";

                var sqls = Directory.GetFiles(path, "*.sql", SearchOption.AllDirectories)
                    .OrderBy(x => x)
                    .ToList();

                // Apply the default SQL (always)
                var initialSql = sqls
                    .Where(x => Path.GetFileName(x) == "0_default.sql")
                    .FirstOrDefault();

                using (var transaction = context.Database.BeginTransaction())
                {
                    context.Database.ExecuteSqlRaw(File.ReadAllText(initialSql));
                    transaction.Commit();
                }

                // Apply migrations if not already applied
                foreach (var sql in sqls.Where(x => Path.GetFileName(x) != "0_default.sql"))
                {
                    var filename = Path.GetFileName(sql);
                    var content = File.ReadAllText(sql);

                    if (!context.Migrations.Any(x => x.Sql == filename))
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;

                        using (var transaction = context.Database.BeginTransaction())
                        {
                            Console.WriteLine($"Applying sql {filename}");
                            context.Database.ExecuteSqlRaw(content);

                            context.Migrations.Add(new()
                            {
                                Date = DateTime.UtcNow,
                                Sql = filename,
                            });

                            context.SaveChanges();
                            context.Database.CommitTransaction();
                        }
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Migrations done");
            }
            catch 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Migrations failed");
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
