ALTER TABLE `gerenciamento_hotel`.`tb_checkin` 
ADD COLUMN `qtd_adultos` INT(11) NULL AFTER `status`,
ADD COLUMN `qtd_criancas` INT(11) NULL AFTER `qtd_adultos`;