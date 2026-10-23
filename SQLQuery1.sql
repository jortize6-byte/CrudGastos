USE GastosDB;
GO

DELETE FROM Gastos;
GO

DBCC CHECKIDENT ('Gastos', RESEED, 0);
GO

INSERT INTO Gastos (Descripcion, Monto, Categoria) VALUES
    (N'Almuerzo ejecutivo',  45.50, N'Comida'),
    (N'Gasolina',           250.00, N'Transporte'),
    (N'Internet residencial', 75.00, N'Servicios');
GO

SELECT * FROM Gastos;