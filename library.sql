SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;


CREATE TABLE `author` (
  `id` int NOT NULL,
  `name` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `book` (
  `id` int NOT NULL,
  `name` varchar(255) NOT NULL,
  `authorID` int NOT NULL,
  `content` text NOT NULL,
  `genreID` int NOT NULL,
  `year_release` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `genre` (
  `id` int NOT NULL,
  `name` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


ALTER TABLE `author`
  ADD PRIMARY KEY (`id`);

ALTER TABLE `book`
  ADD PRIMARY KEY (`id`),
  ADD KEY `genreID` (`genreID`),
  ADD KEY `authorID` (`authorID`);

ALTER TABLE `genre`
  ADD PRIMARY KEY (`id`);


ALTER TABLE `author`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

ALTER TABLE `book`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

ALTER TABLE `genre`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;


ALTER TABLE `book`
  ADD CONSTRAINT `book_ibfk_1` FOREIGN KEY (`authorID`) REFERENCES `author` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  ADD CONSTRAINT `book_ibfk_2` FOREIGN KEY (`genreID`) REFERENCES `genre` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

INSERT INTO `author` (`id`, `name`) VALUES
(1, 'Стивен Кинг'),
(2, 'Звездная Елена'),
(3, 'Бушков Александр'),
(4, 'Чейз Джеймс Хедли'),
(5, 'Гоголь Николай Васильевич'),
(10, 'Дравин Игорь');

INSERT INTO `genre` (`id`, `name`) VALUES
(1, 'Триллер'),
(2, 'Боевик'),
(3, 'Детектив'),
(4, 'Фэнтези'),
(5, 'Ужасы'),
(6, 'Проза');

INSERT INTO `book` (`id`, `name`, `authorID`, `content`, `genreID`, `year_release`) VALUES
(1, 'Оно', 5, 'Роман Стивена Кинга «Оно» написан в жанре ужасов. Это одно из наиболее длинных произведений, им написанных.', 1, 1986),
(2, 'Кладбище домашних животных', 1, 'Стивен Кинг известен всем своими поражающими воображение и ужасающими произведениями. Одним из самых страшных считается «Кладбище домашних животных».', 1, 1983),
(3, 'Тайга и зона', 3, 'Иногда хочется почувствовать опасность, но так, чтобы настоящего риска для здоровья и жизни не было. И вот тогда на помощь приходят книги, написанные реалистично и вызывающие ощущение погружения в описываемые события', 2, 2003),
(4, 'Весь мир в кармане', 4, 'Разбогатеть мечтают многие. Редко кому удается это сделать, поэтому есть те, кто решает прийти к богатству незаконным путем.', 3, 1959),
(5, 'Ты шутишь, наверное?', 4, 'Преступление всегда врывается в жизнь человека неожиданно. Именно из-за фактора неожиданности оно может доставить ему множество неприятностей', 3, 1979),
(6, 'Мертвые игры', 2, 'Тяжело устроиться в неизвестной тебе обстановке, особенно, когда ты совсем не похожа на окружающих. Словно белая ворона. Но если у тебя есть своё мнение и решимость вкупе с природным обаянием, то всё может быть не так плохо.', 4, 2014),
(7, 'Стрелок', 1, '«Стрелок» - первая книга из цикла о Тёмной Башне Стивена Кинга. На написание романа потребовалось 10 лет, а на весь цикл ушло около 30.', 4, 1982),
(8, 'Дверь в чужую осень', 3, 'Великая Отечественная война давно закончилась, но эхо ее еще долго будет раздаваться в наших сердцах. Это было тяжелое и трудное время, и взгляд на эту войну тоже разный. ', 5, 2015),
(9, 'Ревизор', 5, 'О существовании взяточничества и лицемерия по отношению к высокопоставленным лицам уже давно всем известно. Так было и много лет назад, о чем свидетельствуют литературные источники. Причем, раньше это даже имело менее скрытые формы, а некоторые считали дачу взятки обязательной', 6, 2010),
(10, 'Все ведьмы - рыжие', 2, 'Елена Звёздная – автор романа-фэнтези «Все ведьмы – рыжие» – написала юмористическую сказку для взрослых. Это нельзя назвать книгой для детей, поскольку там присутствует юмор, касающийся интимных тем, хоть и в немного завуалированной форме.', 4, 2014);
