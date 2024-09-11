-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 11-09-2024 a las 02:47:19
-- Versión del servidor: 10.4.25-MariaDB
-- Versión de PHP: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmocerutti`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `id_contrato` int(11) NOT NULL,
  `id_inmueble` int(11) NOT NULL,
  `id_inquilino` int(11) NOT NULL,
  `precio` decimal(10,2) NOT NULL,
  `FechaInicio` datetime NOT NULL,
  `FechaFin` datetime NOT NULL,
  `Estado` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`id_contrato`, `id_inmueble`, `id_inquilino`, `precio`, `FechaInicio`, `FechaFin`, `Estado`) VALUES
(15, 37, 19, '222222.00', '2024-09-04 00:00:00', '2024-09-19 00:00:00', 0),
(16, 39, 19, '22.00', '2024-09-06 00:00:00', '2024-09-25 00:00:00', 1),
(17, 41, 21, '2222.00', '2024-09-21 00:00:00', '2024-09-20 00:00:00', 0),
(18, 43, 28, '300.00', '2024-09-01 00:00:00', '2024-09-30 00:00:00', 1),
(19, 42, 21, '22.00', '2024-09-06 00:00:00', '2024-09-13 00:00:00', 0),
(20, 38, 27, '332.00', '2024-09-19 00:00:00', '2024-09-27 00:00:00', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `id_inmueble` int(11) NOT NULL,
  `Direccion` varchar(255) NOT NULL,
  `Uso` varchar(50) NOT NULL,
  `Tipo` varchar(50) NOT NULL,
  `Ambientes` int(11) NOT NULL,
  `Latitud` decimal(9,6) NOT NULL,
  `Longitud` decimal(9,6) NOT NULL,
  `Precio` decimal(18,2) NOT NULL,
  `id_propietario` int(11) NOT NULL,
  `Estado` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`id_inmueble`, `Direccion`, `Uso`, `Tipo`, `Ambientes`, `Latitud`, `Longitud`, `Precio`, `id_propietario`, `Estado`) VALUES
(37, 'Chacabuco 828', 'Comercial', 'Casa', 3, '45.000000', '33.000000', '22222.00', 1, 1),
(38, 'Peru 213', 'comercial', 'Casa', 3, '45.000000', '33.000000', '111111.00', 1, 1),
(39, 'lamadrid 22', 'Comercial', 'Otro', 10, '45.000000', '33.000000', '2222.00', 11, 1),
(40, 'Entre rio 222', 'Comercial', 'Departamento', 3, '45.000000', '33.000000', '111111.00', 1, 0),
(41, 'Entre rio 222', 'Residencial', 'Departamento', 3, '45.000000', '33.000000', '111111.00', 1, 1),
(42, 'Peru 213', 'Residencial', 'Departamento', 3, '45.000000', '33.000000', '111111.00', 1, 1),
(43, 'Espania 920', 'Residencial', 'Casa', 5, '45.000000', '33.000000', '44444.00', 16, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `id_inquilino` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Dni` varchar(20) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Telefono` varchar(15) NOT NULL,
  `Estado` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`id_inquilino`, `Nombre`, `Apellido`, `Dni`, `Email`, `Telefono`, `Estado`) VALUES
(1, 'Alberto', 'cerutti', '3333', 'bruno@gmail.com', '123213', 0),
(19, 'Juan Gabriel Francisco', 'García Flores', '555', 'ejemplo@ejemplo.mx', '5553428400', 0),
(21, 'tomas ', 'shelby', '33333', 's@gmail.com', '5555', 1),
(22, 'pepe', 'Gomez', '1231231', 'alfonpepe@gmail.com', '08000', 1),
(23, 'Sofía', 'Martínez', '56789012', 'sofia.martinez@example.com', '567890123', 1),
(24, 'Pedro', 'García', '67890123', 'pedro.garcia@example.com', '678901234', 1),
(25, 'Lucía', 'Hernández', '78901234', 'lucia.hernandez@example.com', '789012345', 1),
(26, 'Diego', 'Suárez', '89012345', 'diego.suarez@example.com', '890123456', 1),
(27, 'Elena', 'Méndez', '90123456', 'elena.mendez@example.com', '901234567', 1),
(28, 'Manuel', 'foxteim', '010101', 'mqd@gmail.com', '25433', 1),
(29, 'Carlos', 'Sainz', '020202020', 'sainz@gmail.com', '123456778', 1),
(30, 'Soledad', 'Silveira', '000', 'ssilveira@gmail.com', '1231', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `id_pago` int(11) NOT NULL,
  `FechaDePago` date DEFAULT NULL,
  `Motivo` varchar(255) NOT NULL,
  `Importe` decimal(10,2) NOT NULL,
  `id_contrato` int(11) NOT NULL,
  `Estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`id_pago`, `FechaDePago`, `Motivo`, `Importe`, `id_contrato`, `Estado`) VALUES
(11, '2024-09-18', '\"alquiler\"', '222.00', 16, 1),
(12, '2024-09-19', 'alquiler', '222.00', 15, 1),
(13, '2024-09-19', '\"alquiler\"', '222.00', 19, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `id_propietario` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Dni` varchar(50) NOT NULL,
  `Email` varchar(50) NOT NULL,
  `telefono` varchar(20) NOT NULL,
  `Estado` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`id_propietario`, `Nombre`, `Apellido`, `Dni`, `Email`, `telefono`, `Estado`) VALUES
(1, 'Alberto', 'cerutti', '1231231', 'bruno@gmail.com', '123213', 0),
(10, 'Gottfried', 'Leibniz', '22222222', 'test@beispiel.de', '030303986300', 0),
(11, 'Juan Gabriel', 'Pérez Lozano', '12345678', 'juan.perez@example.com', '123456789', 1),
(12, 'María', 'González', '87654321', 'maria.gonzalez@example.com', '987654321', 1),
(13, 'Carlos', 'Ramírez', '23456789', 'carlos.ramirez@example.com', '234567890', 1),
(14, 'Ana', 'López', '34567890', 'ana.lopez@example.com', '345678901', 1),
(15, 'Luis', 'Fernández', '45678901', 'luis.fernandez@example.com', '456789012', 0),
(16, 'Manuel', 'Hernandez Garcia', '666555', 'hernandez@gmail.com', '080099', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Clave` varchar(255) NOT NULL,
  `AvatarUrl` varchar(255) DEFAULT NULL,
  `Rol` varchar(50) NOT NULL,
  `Estado` tinyint(1) DEFAULT 1,
  `Salt` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`Id`, `Nombre`, `Apellido`, `Email`, `Clave`, `AvatarUrl`, `Rol`, `Estado`, `Salt`) VALUES
(27, 'Bruno', 'Cerutti ', 'bruno@gmail.com', '5b2WEUbpgLiQYT8p9iXIZGGh///G8aDj9KS/1dUiDWE=', '/avatars/22bae46b-a339-4081-8e90-a8b67dcfdafb_man.png', 'Administrador', 1, 'PexnprQ/8INRqtwsj7xNZ6IJMLzQ2zbbcjk+DSty0Sg='),
(28, 'Belen', 'Rosas', 'belen@gmail.com', '6pzzo5qB03uQINtkSmaQteR0KFCVjPjMpY87U4A1z1k=', '/avatars/9e9b2144-19fc-4632-a58a-2f35aeded6ac_panda.png', 'Empleado', 1, 'ZKc92yRYvid31T1IJzgf159UqrviYQ4+rvAOv2QmcrY='),
(29, 'pepe', 'alfonso', 'pepe@gmail.com', 'X+J9u5iKzmKWWRfEvElPHu3Vmg4+vpZc/BL7UHa3Fz4=', '', 'Empleado', 0, '8RlLBufXaWLkTc/tR25WBp1y4TQykORWlTWOcgO1jkY=');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`id_contrato`),
  ADD KEY `id_inmueble` (`id_inmueble`),
  ADD KEY `id_inquilino` (`id_inquilino`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`id_inmueble`),
  ADD KEY `id_propietario` (`id_propietario`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`id_inquilino`),
  ADD UNIQUE KEY `Dni` (`Dni`),
  ADD UNIQUE KEY `Email` (`Email`),
  ADD UNIQUE KEY `Telefono` (`Telefono`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`id_pago`),
  ADD KEY `fk_contrato` (`id_contrato`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`id_propietario`),
  ADD UNIQUE KEY `Dni` (`Dni`),
  ADD UNIQUE KEY `Email` (`Email`),
  ADD UNIQUE KEY `telefono` (`telefono`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `id_contrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `id_inmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=44;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `id_inquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `id_pago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `id_propietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=30;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `contrato_ibfk_1` FOREIGN KEY (`id_inmueble`) REFERENCES `inmueble` (`id_inmueble`),
  ADD CONSTRAINT `contrato_ibfk_2` FOREIGN KEY (`id_inquilino`) REFERENCES `inquilino` (`id_inquilino`);

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `inmueble_ibfk_1` FOREIGN KEY (`id_propietario`) REFERENCES `propietario` (`id_propietario`);

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `fk_contrato` FOREIGN KEY (`id_contrato`) REFERENCES `contrato` (`id_contrato`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
