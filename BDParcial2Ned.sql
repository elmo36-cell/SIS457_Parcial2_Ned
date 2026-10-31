-- DDL
CREATE DATABASE Parcial2Ned;
GO
USE master;
GO
CREATE LOGIN usrparcial2 WITH PASSWORD = N'12345678',
    DEFAULT_DATABASE = Parcial2Ned,
    CHECK_EXPIRATION = OFF,
    CHECK_POLICY = OFF;
GO
USE Parcial2Ned;
GO
CREATE USER usrparcial2 FOR LOGIN usrparcial2;
GO
ALTER ROLE [db_owner] ADD MEMBER usrparcial2;
GO

DROP TABLE IF EXISTS HorarioPrograma;
DROP TABLE IF EXISTS Canal;
GO

CREATE TABLE Canal (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  nombre VARCHAR(50) NOT NULL,
  frecuencia VARCHAR(20) NOT NULL
);
GO

CREATE TABLE HorarioPrograma (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  idCanal INT NOT NULL,
  titulo VARCHAR(100) NOT NULL,
  horaInicio TIME NOT NULL,
  horaFin TIME NOT NULL,
  diaSemana VARCHAR(20) NOT NULL,
  conductorPrincipal VARCHAR(100) NOT NULL,
  CONSTRAINT fk_HorarioPrograma_Canal FOREIGN KEY (idCanal) REFERENCES Canal(id)
);
GO

ALTER TABLE Canal ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Canal ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Canal ADD estado SMALLINT NOT NULL DEFAULT 1; -- -1: Eliminado, 0: Inactivo, 1: Activo
GO

ALTER TABLE HorarioPrograma ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE HorarioPrograma ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE HorarioPrograma ADD estado SMALLINT NOT NULL DEFAULT 1; -- -1: Eliminado, 0: Inactivo, 1: Activo
GO

DROP PROC IF EXISTS paCanalListar;
GO
CREATE PROC paCanalListar @parametro VARCHAR(50) AS
  SELECT id, nombre, frecuencia, usuarioRegistro, fechaRegistro, estado
  FROM Canal
  WHERE estado = 1 AND nombre + frecuencia LIKE '%' + REPLACE(@parametro, ' ', '%') + '%'
  ORDER BY nombre;
GO
EXEC paCanalListar 'Canal 7';
GO

DROP PROC IF EXISTS paHorarioProgramaListar;
GO
CREATE PROC paHorarioProgramaListar @parametro VARCHAR(50) AS
  SELECT h.id, h.idCanal, c.nombre AS canal, h.titulo, h.horaInicio, h.horaFin, h.diaSemana, h.conductorPrincipal, h.usuarioRegistro, h.fechaRegistro, h.estado
  FROM HorarioPrograma h
  INNER JOIN Canal c ON c.id = h.idCanal
  WHERE h.estado = 1 AND h.titulo + h.conductorPrincipal + c.nombre LIKE '%' + REPLACE(@parametro, ' ', '%') + '%'
  ORDER BY h.titulo;
GO
EXEC paHorarioProgramaListar 'Cine';
GO

-- DML (Insercion de datos de prueba)
INSERT INTO Canal (nombre, frecuencia) VALUES 
('Bolivia TV', 'Canal 7'),
('Red Uno', 'Canal 13'),
('ATB', 'Canal 9');
GO

INSERT INTO HorarioPrograma (idCanal, titulo, horaInicio, horaFin, diaSemana, conductorPrincipal) VALUES 
(1, 'Noticiero Matutino', '06:30:00', '08:00:00', 'Lunes a Viernes', 'Juan Perez'),
(2, 'Cine Center', '14:00:00', '16:00:00', 'Sábado', 'María Gómez'),
(3, 'Teleadebate', '20:00:00', '21:30:00', 'Domingo', 'Carlos Soria');
GO

SELECT * FROM Canal;
SELECT * FROM HorarioPrograma;
