-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 01-09-2024 a las 08:36:16
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
(15, 37, 19, '222222.00', '2024-09-20 00:00:00', '2024-09-19 00:00:00', 1),
(16, 39, 19, '22.00', '2024-09-06 00:00:00', '2024-09-24 00:00:00', 1),
(17, 41, 21, '2222.00', '2024-09-21 00:00:00', '2024-09-20 00:00:00', 1);

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
(39, 'lamadrid 22', 'Comercial', 'Casa', 3, '45.000000', '33.000000', '2222.00', 1, 1),
(40, 'Entre rio 222', 'Comercial', 'Departamento', 3, '45.000000', '33.000000', '111111.00', 1, 0),
(41, 'Entre rio 222', 'Residencial', 'Departamento', 3, '45.000000', '33.000000', '111111.00', 1, 1),
(42, 'Peru 213', 'Residencial', 'Departamento', 3, '45.000000', '33.000000', '111111.00', 1, 1);

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
(19, 'Juan Francisco', 'García Flores', '555', 'ejemplo@ejemplo.mx', '5553428400', 1),
(21, 'tomas ', 'shelby', '33333', 's@gmail.com', '123456', 1),
(22, 'pepe', 'Gomez', '1231231', 'alfonpepe@gmail.com', '08000', 1),
(23, 'Sofía', 'Martínez', '56789012', 'sofia.martinez@example.com', '567890123', 1),
(24, 'Pedro', 'García', '67890123', 'pedro.garcia@example.com', '678901234', 1),
(25, 'Lucía', 'Hernández', '78901234', 'lucia.hernandez@example.com', '789012345', 1),
(26, 'Diego', 'Suárez', '89012345', 'diego.suarez@example.com', '890123456', 1),
(27, 'Elena', 'Méndez', '90123456', 'elena.mendez@example.com', '901234567', 1);

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
(1, 'Alberto', 'cerutti', '1231231', 'bruno@gmail.com', '123213', 1),
(10, 'Gottfried', 'Leibniz', '22222222', 'test@beispiel.de', '030303986300', 0),
(11, 'Juan', 'Pérez', '12345678', 'juan.perez@example.com', '123456789', 1),
(12, 'María', 'González', '87654321', 'maria.gonzalez@example.com', '987654321', 1),
(13, 'Carlos', 'Ramírez', '23456789', 'carlos.ramirez@example.com', '234567890', 1),
(14, 'Ana', 'López', '34567890', 'ana.lopez@example.com', '345678901', 1),
(15, 'Luis', 'Fernández', '45678901', 'luis.fernandez@example.com', '456789012', 1);

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
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`id_propietario`),
  ADD UNIQUE KEY `Dni` (`Dni`),
  ADD UNIQUE KEY `Email` (`Email`),
  ADD UNIQUE KEY `telefono` (`telefono`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `id_contrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `id_inmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=43;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `id_inquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=28;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `id_propietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

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
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
