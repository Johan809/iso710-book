CREATE DATABASE ISO710
GO

USE ISO710
GO

CREATE TABLE Miembro (
    MiembroId INT PRIMARY KEY IDENTITY(1,1),
    NombreCompleto VARCHAR(100) NOT NULL,
    Correo VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20),
    Direccion VARCHAR(255),
    FechaRegistro DATETIME DEFAULT GETDATE()
);

CREATE TABLE Prestamo (
    PrestamoId INT PRIMARY KEY IDENTITY(1,1),
    MiembroId INT NOT NULL,
    ISBN VARCHAR(13) NOT NULL,
    Titulo VARCHAR(255) NOT NULL,
    FechaPrestamo DATETIME DEFAULT GETDATE(),
    FechaDevolucion DATETIME,
    Devuelto BIT DEFAULT 0,

    -- Clave foránea para relacionar los préstamos con los miembros
    CONSTRAINT FK_Prestamo_Miembro FOREIGN KEY (MiembroId)
    REFERENCES Miembro(MiembroId)
);
GO
