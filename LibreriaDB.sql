-- Creación de la base de datos y uso de la misma
CREATE DATABASE LibreriaDB;
GO

USE LibreriaDB;
GO

-- Tabla de Rol
CREATE TABLE Rol (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50) NOT NULL
);
GO

-- Tabla de Usuario
CREATE TABLE Usuario (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50) NOT NULL,
    Apellido NVARCHAR(50) NOT NULL,
    Correo NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(15),
    RolID INT,
    Password NVARCHAR(255) NOT NULL,
    CONSTRAINT FK_Usuario_Rol FOREIGN KEY (RolID) REFERENCES Rol(ID)
);
GO

-- Tabla de Tarjeta
CREATE TABLE Tarjeta (
    ID INT PRIMARY KEY IDENTITY(1,1),
    UsuarioID INT,
    Numero NVARCHAR(20) NOT NULL,
    NombreTitular NVARCHAR(50) NOT NULL,
    FechaExpiracion DATE NOT NULL,
    CVV NVARCHAR(4) NOT NULL,
    CONSTRAINT FK_Tarjeta_Usuario FOREIGN KEY (UsuarioID) REFERENCES Usuario(ID)
);
GO

-- Tabla Autor
CREATE TABLE Autor (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL
);
GO

-- Tabla Genero
CREATE TABLE Genero (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50) NOT NULL
);
GO

-- Tabla Formato
CREATE TABLE Formato (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50) NOT NULL
);
GO

-- Tabla Libro
CREATE TABLE Libro (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    AutorID INT,
    GeneroID INT,
    FormatoID INT,
    Descripcion NVARCHAR(255),
    Precio DECIMAL(10, 2) NOT NULL,
    Stock INT NOT NULL,
    RutaFoto NVARCHAR(255),
    CONSTRAINT FK_Libro_Autor FOREIGN KEY (AutorID) REFERENCES Autor(ID),
    CONSTRAINT FK_Libro_Genero FOREIGN KEY (GeneroID) REFERENCES Genero(ID),
    CONSTRAINT FK_Libro_Formato FOREIGN KEY (FormatoID) REFERENCES Formato(ID)
);
GO

-- Tabla Compra
CREATE TABLE Compra (
    ID INT PRIMARY KEY IDENTITY(1,1),
    UsuarioID INT,
    Total DECIMAL(10, 2) NOT NULL,
    FechaCompra DATETIME NOT NULL,
    CONSTRAINT FK_Compra_Usuario FOREIGN KEY (UsuarioID) REFERENCES Usuario(ID)
);
GO


ALTER TABLE dbo.Libro
ALTER COLUMN Descripcion VARCHAR(MAX);


-- TABLA MAESTRO
ALTER TABLE Maestro DROP COLUMN UsuarioId1;

-- TABLA DETALLE
CREATE TABLE Detalle (
    DetalleId INT PRIMARY KEY IDENTITY(1,1),
    MaestroId INT,
    ProductoId INT,
    CantidadCompra INT,
    PrecioCompra DECIMAL(10, 2),
    Impuesto DECIMAL(10, 2),
);

-- TABLA CARRITO
ALTER TABLE Carrito DROP COLUMN LibroId

CREATE PROCEDURE PagarCarrito_SP
    @UsuarioId BIGINT
AS
BEGIN
    DECLARE @TotalFactura AS DECIMAL(18, 2);
    DECLARE @MaestroId AS BIGINT;

    -- Calcula el total de la factura y asegúrate de que no sea NULL
    SELECT @TotalFactura = ISNULL(SUM(P.Precio * C.Cantidad) + SUM(P.Precio * C.Cantidad) * 0.13, 0)
    FROM Carrito C
    INNER JOIN Libro P ON C.ProductoId = P.ID
    WHERE C.UsuarioId = @UsuarioId;

    -- Inserta en la tabla Maestro si @TotalFactura tiene un valor válido
    IF @TotalFactura > 0
    BEGIN
        INSERT INTO Maestro(UsuarioId, TotalFactura, FechaCompra)
        VALUES(@UsuarioId, @TotalFactura, GETDATE());

        -- Obtén el ID de la última inserción
        SET @MaestroId = SCOPE_IDENTITY();

        -- Inserta los detalles en la tabla Detalle
        INSERT INTO dbo.Detalle(MaestroId, ProductoId, CantidadCompra, PrecioCompra, Impuesto)
        SELECT @MaestroId, C.ProductoId, C.Cantidad, P.Precio, (C.Cantidad * P.Precio) * 0.13
        FROM Carrito C
        INNER JOIN Libro P ON C.ProductoId = P.ID
        WHERE C.UsuarioId = @UsuarioId;

        -- Actualiza el stock en la tabla Libro
        UPDATE P
        SET P.Stock -= C.Cantidad
        FROM Libro P
        INNER JOIN Carrito C ON C.ProductoId = P.ID
        WHERE C.UsuarioId = @UsuarioId;

        -- Elimina los artículos del carrito para este usuario
        DELETE FROM dbo.Carrito
        WHERE UsuarioId = @UsuarioId;
    END
    ELSE
    BEGIN
        RAISERROR('No se encontraron artículos en el carrito para el usuario especificado.', 16, 1);
    END
END
GO



CREATE PROCEDURE sp_RegistrarCarrito
    @UsuarioId INT,
    @ProductoId INT,
    @Cantidad INT,
    @FechaCarrito DATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar si ya existe un registro con el mismo UsuarioId y ProductoId
    DECLARE @ExistingCarritoId INT;

    SELECT @ExistingCarritoId = CarritoId
    FROM Carrito
    WHERE UsuarioId = @UsuarioId AND ProductoId = @ProductoId;

    IF @ExistingCarritoId IS NOT NULL
    BEGIN
        -- Si existe, actualizar la cantidad
        UPDATE Carrito
        SET Cantidad = @Cantidad
        WHERE CarritoId = @ExistingCarritoId;

        -- Opcional: Devolver un mensaje indicando que se actualizó
        SELECT 'Carrito actualizado con éxito.' AS Mensaje;
    END
    ELSE
    BEGIN
        -- Si no existe, insertar un nuevo registro
        INSERT INTO Carrito (UsuarioId, ProductoId, Cantidad, FechaCarrito)
        VALUES (@UsuarioId, @ProductoId, @Cantidad, @FechaCarrito);

        -- Opcional: Devolver un mensaje indicando que se insertó
        SELECT 'Carrito registrado con éxito.' AS Mensaje;
    END
END

CREATE PROCEDURE sp_EliminarProductoCarrito
    @CarritoId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar si el producto existe antes de eliminarlo
    IF EXISTS (SELECT 1 FROM Carrito WHERE CarritoId = @CarritoId)
    BEGIN
        DELETE FROM Carrito
        WHERE CarritoId = @CarritoId;
    END
END
GO



