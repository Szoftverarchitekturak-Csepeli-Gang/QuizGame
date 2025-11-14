/*
  Warnings:

  - You are about to drop the column `question_bank_id` on the `Question` table. All the data in the column will be lost.
  - You are about to drop the column `owner_user_id` on the `Question_Bank` table. All the data in the column will be lost.
  - You are about to drop the column `host_id` on the `Room` table. All the data in the column will be lost.
  - You are about to drop the column `question_bank_id` on the `Room` table. All the data in the column will be lost.
  - A unique constraint covering the columns `[hostId]` on the table `Room` will be added. If there are existing duplicate values, this will fail.
  - Added the required column `questionBankId` to the `Question` table without a default value. This is not possible if the table is not empty.
  - Added the required column `ownerId` to the `Question_Bank` table without a default value. This is not possible if the table is not empty.
  - Added the required column `hostId` to the `Room` table without a default value. This is not possible if the table is not empty.
  - Added the required column `questionBankId` to the `Room` table without a default value. This is not possible if the table is not empty.

*/
-- DropForeignKey
ALTER TABLE `Question` DROP FOREIGN KEY `Question_question_bank_id_fkey`;

-- DropForeignKey
ALTER TABLE `Room` DROP FOREIGN KEY `Room_question_bank_id_fkey`;

-- DropIndex
DROP INDEX `Question_question_bank_id_fkey` ON `Question`;

-- DropIndex
DROP INDEX `Room_host_id_key` ON `Room`;

-- DropIndex
DROP INDEX `Room_question_bank_id_fkey` ON `Room`;

-- AlterTable
ALTER TABLE `Question` DROP COLUMN `question_bank_id`,
    ADD COLUMN `questionBankId` INTEGER NOT NULL;

-- AlterTable
ALTER TABLE `Question_Bank` DROP COLUMN `owner_user_id`,
    ADD COLUMN `ownerId` INTEGER NOT NULL;

-- AlterTable
ALTER TABLE `Room` DROP COLUMN `host_id`,
    DROP COLUMN `question_bank_id`,
    ADD COLUMN `hostId` INTEGER NOT NULL,
    ADD COLUMN `questionBankId` INTEGER NOT NULL;

-- CreateIndex
CREATE UNIQUE INDEX `Room_hostId_key` ON `Room`(`hostId`);

-- AddForeignKey
ALTER TABLE `Question` ADD CONSTRAINT `Question_questionBankId_fkey` FOREIGN KEY (`questionBankId`) REFERENCES `Question_Bank`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Room` ADD CONSTRAINT `Room_questionBankId_fkey` FOREIGN KEY (`questionBankId`) REFERENCES `Question_Bank`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;
