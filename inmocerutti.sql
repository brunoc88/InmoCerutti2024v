-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 14-09-2024 a las 19:02:33
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
  `Estado` tinyint(1) DEFAULT 1,
  `id_usuario` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`id_contrato`, `id_inmueble`, `id_inquilino`, `precio`, `FechaInicio`, `FechaFin`, `Estado`, `id_usuario`) VALUES
(34, 68, 35, '104.00', '2024-09-14 00:00:00', '2024-10-14 00:00:00', 1, 28),
(35, 70, 34, '222.00', '2024-09-14 00:00:00', '2024-10-11 00:00:00', 1, 27),
(36, 72, 34, '100.00', '2024-09-14 00:00:00', '2024-11-14 00:00:00', 1, 27);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `id_inmueble` int(11) NOT NULL,
  `Direccion` varchar(255) NOT NULL,
  `Uso` varchar(50) NOT NULL,
  `Ambientes` int(11) NOT NULL,
  `Latitud` decimal(9,6) NOT NULL,
  `Longitud` decimal(9,6) NOT NULL,
  `Precio` decimal(18,2) NOT NULL,
  `id_propietario` int(11) NOT NULL,
  `Estado` tinyint(1) DEFAULT 1,
  `id_tipoInmueble` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`id_inmueble`, `Direccion`, `Uso`, `Ambientes`, `Latitud`, `Longitud`, `Precio`, `id_propietario`, `Estado`, `id_tipoInmueble`) VALUES
(68, 'Montevideo 33', 'Residencial', 1, '12.000000', '22.000000', '102.00', 20, 1, 32),
(69, 'Asuncion 33', 'Residencial', 3, '21.000000', '33.000000', '222.00', 21, 1, 32),
(70, 'Lima 13', 'Residencial', 3, '21.000000', '33.000000', '222.00', 22, 1, 34),
(71, 'Madrid 77', 'Residencial', 4, '21.000000', '33.000000', '222.00', 22, 1, 34),
(72, 'Caracas', 'Comercial', 2, '21.000000', '33.000000', '100.00', 20, 1, 39);

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
(34, 'Jose Ramon', 'Lopez', '111', 'josera@gmail.com', '0101', 1),
(35, 'Maria Angeles', 'Gomez Bolanio', '2020', 'mariag12@gmail.com', '20202', 1),
(36, 'Juan Gabriel', 'Osorio', '414', 'juanga@gmail.com', '3030', 1),
(37, 'Raquel', 'Godoy', '555', 'raquelgodoy@gmail.com', '4040', 1);

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
(40, '2024-09-14', '\"alquiler\"', '222.00', 34, 1),
(41, '2024-09-14', '\"alquiler\"', '222.00', 35, 1),
(42, '2024-09-14', '\"alquiler\"', '222.00', 36, 1),
(43, '2024-10-14', '\"Pago\"', '222.00', 35, 1);

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
(20, 'Antonio', 'Torres', '777', 'antonytorres@gmail.com', '5050', 1),
(21, 'Ana Maria', 'Costanzo', '3939', 'constanzoana@gmail.com', '5656', 1),
(22, 'Pedro', 'Hernandez', '3838', 'pepehernandez@gmail.com', '6060', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipoinmueble`
--

CREATE TABLE `tipoinmueble` (
  `id_tipoInmueble` int(11) NOT NULL,
  `tipoNombre` varchar(50) NOT NULL,
  `Estado` tinyint(1) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `tipoinmueble`
--

INSERT INTO `tipoinmueble` (`id_tipoInmueble`, `tipoNombre`, `Estado`) VALUES
(32, 'Departamento', 1),
(34, 'Casa', 1),
(35, 'Otro...', 1),
(36, 'Hotel', 1),
(38, 'Deposito', 1),
(39, 'Local', 1);

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
(27, 'Bruno ', 'Cerutti ', 'bruno@gmail.com', 'iVKjbYMxRZvOVIft4Gx3dVA1LFJLWh9gcYKDCUvUxg8=', '/avatars/33d418ca-86f1-450b-8ed1-b9c1109eafa0_profile1.png', 'Administrador', 1, 'hd02krrPtn9VYDMFNRQDKurH1CXVbxrZtww6S+noYvQ='),
(28, 'Belen ', 'Rosas ', 'belen@gmail.com', 'f3KHA8D3/qhWrA/d1VzB2LkpTNI1I3cFgkqyFs0UHJA=', '/avatars/7c45e777-696d-4a43-a9f2-294077bc4128_panda.png', 'Empleado', 1, 'bJr5k9Wz7zWXZq+6LEoMLwhmLbJbcWe/ddQU2LeOOS4=');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`id_contrato`),
  ADD KEY `id_inmueble` (`id_inmueble`),
  ADD KEY `id_inquilino` (`id_inquilino`),
  ADD KEY `FK_Contrato_Usuario` (`id_usuario`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`id_inmueble`),
  ADD UNIQUE KEY `Direccion_2` (`Direccion`),
  ADD KEY `id_propietario` (`id_propietario`),
  ADD KEY `FK_Inmueble_TipoInmueble` (`id_tipoInmueble`);

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
-- Indices de la tabla `tipoinmueble`
--
ALTER TABLE `tipoinmueble`
  ADD PRIMARY KEY (`id_tipoInmueble`),
  ADD UNIQUE KEY `Nombre` (`tipoNombre`);

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
  MODIFY `id_contrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=37;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `id_inmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=73;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `id_inquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=38;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `id_pago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=44;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `id_propietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT de la tabla `tipoinmueble`
--
ALTER TABLE `tipoinmueble`
  MODIFY `id_tipoInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=40;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=39;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `FK_Contrato_Usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`Id`),
  ADD CONSTRAINT `contrato_ibfk_1` FOREIGN KEY (`id_inmueble`) REFERENCES `inmueble` (`id_inmueble`),
  ADD CONSTRAINT `contrato_ibfk_2` FOREIGN KEY (`id_inquilino`) REFERENCES `inquilino` (`id_inquilino`);

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `FK_Inmueble_TipoInmueble` FOREIGN KEY (`id_tipoInmueble`) REFERENCES `tipoinmueble` (`id_tipoInmueble`),
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
