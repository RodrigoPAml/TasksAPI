IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Migrations') AND type = 'U')
BEGIN
    CREATE TABLE Migrations (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Sql NVARCHAR(MAX) NOT NULL,
        Date DATETIME NOT NULL
    );
END;
