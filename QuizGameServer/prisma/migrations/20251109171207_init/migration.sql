-- CreateTable
CREATE TABLE `Question_Bank` (
    `id` INTEGER NOT NULL AUTO_INCREMENT,
    `owner_user_id` INTEGER NOT NULL,
    `title` VARCHAR(191) NOT NULL,
    `public` BOOLEAN NOT NULL,

    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `Question` (
    `id` INTEGER NOT NULL AUTO_INCREMENT,
    `question_bank_id` INTEGER NOT NULL,
    `text` VARCHAR(191) NOT NULL,
    `optionA` VARCHAR(191) NOT NULL,
    `optionB` VARCHAR(191) NOT NULL,
    `optionC` VARCHAR(191) NOT NULL,
    `optionD` VARCHAR(191) NOT NULL,
    `correctAnswer` INTEGER NOT NULL,

    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `Room` (
    `id` INTEGER NOT NULL,
    `host_id` INTEGER NOT NULL,
    `question_bank_id` INTEGER NOT NULL,
    `isOpen` BOOLEAN NOT NULL,

    UNIQUE INDEX `Room_host_id_key`(`host_id`),
    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- AddForeignKey
ALTER TABLE `Question` ADD CONSTRAINT `Question_question_bank_id_fkey` FOREIGN KEY (`question_bank_id`) REFERENCES `Question_Bank`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `Room` ADD CONSTRAINT `Room_question_bank_id_fkey` FOREIGN KEY (`question_bank_id`) REFERENCES `Question_Bank`(`id`) ON DELETE RESTRICT ON UPDATE CASCADE;
