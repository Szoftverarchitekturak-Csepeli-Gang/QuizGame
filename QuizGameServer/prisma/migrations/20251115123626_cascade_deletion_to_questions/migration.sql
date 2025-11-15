-- DropForeignKey
ALTER TABLE `Question` DROP FOREIGN KEY `Question_questionBankId_fkey`;

-- DropIndex
DROP INDEX `Question_questionBankId_fkey` ON `Question`;

-- AddForeignKey
ALTER TABLE `Question` ADD CONSTRAINT `Question_questionBankId_fkey` FOREIGN KEY (`questionBankId`) REFERENCES `Question_Bank`(`id`) ON DELETE CASCADE ON UPDATE CASCADE;
