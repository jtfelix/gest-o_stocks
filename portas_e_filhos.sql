-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Tempo de geração: 26-Mar-2023 às 13:40
-- Versão do servidor: 10.4.27-MariaDB
-- versão do PHP: 8.1.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Banco de dados: `portas_e_filhos`
--

-- --------------------------------------------------------

--
-- Estrutura da tabela `encomendas`
--

CREATE TABLE `encomendas` (
  `ID` int(11) NOT NULL,
  `ID_materiais` int(11) NOT NULL,
  `ID_fornecedor` int(11) NOT NULL,
  `quantidade` int(11) NOT NULL,
  `valor` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Extraindo dados da tabela `encomendas`
--

INSERT INTO `encomendas` (`ID`, `ID_materiais`, `ID_fornecedor`, `quantidade`, `valor`) VALUES
(2, 15, 2, 3, '9.00'),
(4, 15, 3, 4, '12.00'),
(5, 15, 2, 2, '6.00'),
(6, 16, 3, 3, '7.00'),
(7, 17, 3, 5, '5.60'),
(8, 17, 2, 7, '7.00');

-- --------------------------------------------------------

--
-- Estrutura da tabela `fornecedores`
--

CREATE TABLE `fornecedores` (
  `ID` int(11) NOT NULL,
  `nome` varchar(50) NOT NULL,
  `morada` varchar(50) NOT NULL,
  `id_tipo_material` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Extraindo dados da tabela `fornecedores`
--

INSERT INTO `fornecedores` (`ID`, `nome`, `morada`, `id_tipo_material`) VALUES
(2, 'Santos e Vale', 'Rua Nacional', 5),
(3, 'André e Filhos', 'Estrada Municipal Estes', 3),
(4, 'Pinto e Filhos', 'Rua André Sardet', 6);

-- --------------------------------------------------------

--
-- Estrutura da tabela `materiais`
--

CREATE TABLE `materiais` (
  `ID` int(11) NOT NULL,
  `nome` varchar(50) NOT NULL,
  `preco` decimal(10,2) NOT NULL,
  `stock` smallint(6) NOT NULL,
  `id_tipo_material` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Extraindo dados da tabela `materiais`
--

INSERT INTO `materiais` (`ID`, `nome`, `preco`, `stock`, `id_tipo_material`) VALUES
(15, 'Painel Vidro 2x1', '3.00', 8, 3),
(16, 'Parafuso Rosca', '2.41', 0, 3),
(17, 'Papel para Mãos', '1.12', 10, 6);

-- --------------------------------------------------------

--
-- Estrutura da tabela `materiais_produtos`
--

CREATE TABLE `materiais_produtos` (
  `ID` int(11) NOT NULL,
  `ID_produto` int(11) NOT NULL,
  `ID_materiais` int(11) NOT NULL,
  `quantidade_material` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Extraindo dados da tabela `materiais_produtos`
--

INSERT INTO `materiais_produtos` (`ID`, `ID_produto`, `ID_materiais`, `quantidade_material`) VALUES
(5, 3, 15, 2),
(8, 2, 17, 4),
(11, 2, 15, 1),
(14, 2, 17, 4),
(15, 2, 16, 4),
(16, 2, 17, 1);

-- --------------------------------------------------------

--
-- Estrutura da tabela `produtos`
--

CREATE TABLE `produtos` (
  `ID` int(11) NOT NULL,
  `nome` varchar(25) NOT NULL,
  `preco_venda` decimal(10,2) NOT NULL,
  `tempo_producao` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Extraindo dados da tabela `produtos`
--

INSERT INTO `produtos` (`ID`, `nome`, `preco_venda`, `tempo_producao`) VALUES
(2, 'Porta Aluminio ', '102.00', 59),
(3, 'Janela Sr Carlos', '100.00', 188);

-- --------------------------------------------------------

--
-- Estrutura da tabela `tipo_material`
--

CREATE TABLE `tipo_material` (
  `ID` int(11) NOT NULL,
  `descricao` varchar(80) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Extraindo dados da tabela `tipo_material`
--

INSERT INTO `tipo_material` (`ID`, `descricao`) VALUES
(3, 'Bricolage'),
(4, 'Vidros'),
(5, 'Material de Escritorios'),
(6, 'Higieneeeeeeeee'),
(8, 'Mecanico '),
(9, 'Plastico');

--
-- Índices para tabelas despejadas
--

--
-- Índices para tabela `encomendas`
--
ALTER TABLE `encomendas`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `encomendas_ibfk_1` (`ID_fornecedor`),
  ADD KEY `encomendas_ibfk_2` (`ID_materiais`);

--
-- Índices para tabela `fornecedores`
--
ALTER TABLE `fornecedores`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `fornecedores_ibfk_1` (`id_tipo_material`);

--
-- Índices para tabela `materiais`
--
ALTER TABLE `materiais`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `materiais_ibfk_1` (`id_tipo_material`);

--
-- Índices para tabela `materiais_produtos`
--
ALTER TABLE `materiais_produtos`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `materiais_produtos_ibfk_1` (`ID_materiais`),
  ADD KEY `materiais_produtos_ibfk_2` (`ID_produto`);

--
-- Índices para tabela `produtos`
--
ALTER TABLE `produtos`
  ADD PRIMARY KEY (`ID`);

--
-- Índices para tabela `tipo_material`
--
ALTER TABLE `tipo_material`
  ADD PRIMARY KEY (`ID`);

--
-- AUTO_INCREMENT de tabelas despejadas
--

--
-- AUTO_INCREMENT de tabela `encomendas`
--
ALTER TABLE `encomendas`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de tabela `fornecedores`
--
ALTER TABLE `fornecedores`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de tabela `materiais`
--
ALTER TABLE `materiais`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT de tabela `materiais_produtos`
--
ALTER TABLE `materiais_produtos`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT de tabela `produtos`
--
ALTER TABLE `produtos`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de tabela `tipo_material`
--
ALTER TABLE `tipo_material`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- Restrições para despejos de tabelas
--

--
-- Limitadores para a tabela `encomendas`
--
ALTER TABLE `encomendas`
  ADD CONSTRAINT `encomendas_ibfk_1` FOREIGN KEY (`ID_fornecedor`) REFERENCES `fornecedores` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `encomendas_ibfk_2` FOREIGN KEY (`ID_materiais`) REFERENCES `materiais` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Limitadores para a tabela `fornecedores`
--
ALTER TABLE `fornecedores`
  ADD CONSTRAINT `fornecedores_ibfk_1` FOREIGN KEY (`id_tipo_material`) REFERENCES `tipo_material` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Limitadores para a tabela `materiais`
--
ALTER TABLE `materiais`
  ADD CONSTRAINT `materiais_ibfk_1` FOREIGN KEY (`id_tipo_material`) REFERENCES `tipo_material` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Limitadores para a tabela `materiais_produtos`
--
ALTER TABLE `materiais_produtos`
  ADD CONSTRAINT `materiais_produtos_ibfk_1` FOREIGN KEY (`ID_materiais`) REFERENCES `materiais` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `materiais_produtos_ibfk_2` FOREIGN KEY (`ID_produto`) REFERENCES `produtos` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
