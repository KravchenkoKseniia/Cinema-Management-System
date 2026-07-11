-- MySQL dump 10.13  Distrib 9.2.0, for Linux (aarch64)
--
-- Host: localhost    Database: Cinema
-- ------------------------------------------------------
-- Server version	9.2.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Genres`
--

DROP TABLE IF EXISTS `Genres`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Genres` (
  `genreID` int NOT NULL AUTO_INCREMENT,
  `genreName` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`genreID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Genres`
--

LOCK TABLES `Genres` WRITE;
/*!40000 ALTER TABLE `Genres` DISABLE KEYS */;
INSERT INTO `Genres` VALUES (1,'Action'),(2,'Comedy'),(3,'Drama'),(4,'Horror'),(5,'Romance');
/*!40000 ALTER TABLE `Genres` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Halls`
--

DROP TABLE IF EXISTS `Halls`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Halls` (
  `hallID` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT NULL,
  `capacity` int DEFAULT NULL,
  PRIMARY KEY (`hallID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Halls`
--

LOCK TABLES `Halls` WRITE;
/*!40000 ALTER TABLE `Halls` DISABLE KEYS */;
INSERT INTO `Halls` VALUES (1,'Hall 1',100),(2,'Hall 2',150),(3,'Hall 3',200);
/*!40000 ALTER TABLE `Halls` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Movies`
--

DROP TABLE IF EXISTS `Movies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Movies` (
  `movieID` int NOT NULL AUTO_INCREMENT,
  `title` varchar(255) DEFAULT NULL,
  `description` text,
  `trailerURL` varchar(255) DEFAULT NULL,
  `releaseDate` date DEFAULT NULL,
  `genreID` int DEFAULT NULL,
  `rating` decimal(10,0) DEFAULT NULL,
  `duration` time DEFAULT NULL,
  `posterURL` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`movieID`),
  KEY `genreID` (`genreID`),
  CONSTRAINT `Movies_ibfk_1` FOREIGN KEY (`genreID`) REFERENCES `Genres` (`genreID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Movies`
--

LOCK TABLES `Movies` WRITE;
/*!40000 ALTER TABLE `Movies` DISABLE KEYS */;
INSERT INTO `Movies` VALUES (1,'The Shawshank Redemption','The Shawshank Redemption is a 1994 American drama film written and directed by Frank Darabont, based on the 1982 Stephen King novella Rita Hayworth and Shawshank Redemption. It tells the story of banker Andy Dufresne (Tim Robbins), who is sentenced to life in Shawshank State Penitentiary for the murders','https://www.youtube.com/watch?v=6hB3S9bIaco','1994-09-23',3,9,'02:22:00','https://upload.wikimedia.org/wikipedia/en/8/81/ShawshankRedemptionMoviePoster.jpg'),(2,'The Godfather','The Godfather is a 1972 American crime film directed by Francis Ford Coppola, who co-wrote the screenplay with Mario Puzo, based on Puzo\'s best-selling 1969 novel of the same name. The film stars Marlon Brando, Al Pacino, James Caan, Richard Castellano, Robert Duvall, Sterling Hayden, John Marley, Richard Conte, and Diane Keaton. It is the first installment in The Godfather trilogy. The story, spanning from 1945 to 1955, chronicles the Corleone family under patriarch Vito Corleone (Brando), focusing on the transformation of his youngest son, Michael Corleone (Pacino), from reluctant family outsider to ruthless mafia boss.','https://www.youtube.com/watch?v=sY1S34973zA','1972-03-24',3,9,'02:55:00','https://upload.wikimedia.org/wikipedia/en/1/1c/Godfather_ver1.jpg'),(3,'The Dark Knight','The Dark Knight is a 2008 superhero film directed, produced, and co-written by Christopher Nolan. Based on the DC Comics character Batman, the film is the second installment of Nolan\'s The Dark Knight Trilogy and a sequel to 2005\'s Batman Begins, starring Christian Bale and supported by Michael Caine, Heath Ledger, Gary Oldman, Aaron Eckhart, Maggie Gyllenhaal, and Morgan Freeman. In the film, Bruce Wayne / Batman (Bale), Police Lieutenant James Gordon (Oldman) and District Attorney Harvey Dent (Eckhart) form an alliance to dismantle organized crime in Gotham City, but are menaced by an anarchistic mastermind known as the Joker (Ledger), who seeks to undermine Batman\'s influence and turn the city to chaos.','https://www.youtube.com/watch?v=EXeTwQWrcwY','2008-07-18',1,9,'02:32:00','https://upload.wikimedia.org/wikipedia/en/8/8a/Dark_Knight.jpg'),(4,'The Lord of the Rings: The Return of the King','The Lord of the Rings: The Return of the King is a 2003 epic fantasy adventure film directed by Peter Jackson, based on the third volume of J. R. R. Tolkien\'s The Lord of the Rings. The film is the final installment in the Lord of the Rings trilogy and was produced by Barrie M. Osborne, Jackson, and Fran Walsh, and written by Walsh, Philippa Boyens, and Jackson. Continuing the plot of The Two Towers, Frodo, Sam, and Gollum are making their final way toward Mount Doom in Mordor in order to destroy the One Ring, unaware of Gollum\'s true intentions, while Gandalf, Aragorn, Legolas, Gimli, and the rest are joining forces together against Sauron and his legions in Minas Tirith.','https://www.youtube.com/watch?v=r5X-hFf6Bwo','2003-12-17',1,9,'03:21:00','https://upload.wikimedia.org/wikipedia/en/9/9d/Lord_of_the_Rings_-_The_Return_of_the_King.jpg'),(5,'Pulp Fiction','Pulp Fiction is a 1994 American neo-noir black comedy crime film written and directed by Quentin Tarantino, who conceived it with Roger Avary. Starring John Travolta, Samuel L. Jackson, Bruce Willis, Tim Roth, Ving Rhames, and Uma Thurman, it tells several stories of criminal Los Angeles. The title refers to the pulp magazines and hardboiled crime novels popular during the mid-20th century, known for their graphic violence and punchy dialogue.','https://www.youtube.com/watch?v=s7EdQ4FqbhY','1994-10-14',2,9,'02:34:00','https://upload.wikimedia.org/wikipedia/en/3/3b/Pulp_Fiction_%281994%29_poster.jpg');
/*!40000 ALTER TABLE `Movies` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Payments`
--

DROP TABLE IF EXISTS `Payments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Payments` (
  `paymentID` int NOT NULL AUTO_INCREMENT,
  `userID` int DEFAULT NULL,
  `amount` decimal(10,0) DEFAULT NULL,
  `date` datetime DEFAULT NULL,
  PRIMARY KEY (`paymentID`),
  KEY `userID` (`userID`),
  CONSTRAINT `Payments_ibfk_1` FOREIGN KEY (`userID`) REFERENCES `Users` (`userID`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Payments`
--

LOCK TABLES `Payments` WRITE;
/*!40000 ALTER TABLE `Payments` DISABLE KEYS */;
INSERT INTO `Payments` VALUES (1,2,100,'2021-05-01 12:00:00'),(2,3,200,'2021-05-02 12:00:00'),(3,4,300,'2021-05-03 12:00:00'),(4,2,100,'2021-05-01 15:00:00'),(5,3,200,'2021-05-02 15:00:00'),(6,4,300,'2021-05-03 15:00:00'),(7,2,100,'2021-05-01 18:00:00'),(8,3,200,'2021-05-02 18:00:00'),(9,4,300,'2021-05-03 18:00:00');
/*!40000 ALTER TABLE `Payments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Roles`
--

DROP TABLE IF EXISTS `Roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Roles` (
  `roleID` int NOT NULL AUTO_INCREMENT,
  `roleName` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`roleID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Roles`
--

LOCK TABLES `Roles` WRITE;
/*!40000 ALTER TABLE `Roles` DISABLE KEYS */;
INSERT INTO `Roles` VALUES (1,'Admin'),(2,'Client');
/*!40000 ALTER TABLE `Roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Sessions`
--

DROP TABLE IF EXISTS `Sessions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Sessions` (
  `sessionID` int NOT NULL AUTO_INCREMENT,
  `movieID` int DEFAULT NULL,
  `startTime` time DEFAULT NULL,
  `endTime` time DEFAULT NULL,
  `date` date DEFAULT NULL,
  `hallID` int DEFAULT NULL,
  `ticketPrice` decimal(10,0) DEFAULT NULL,
  PRIMARY KEY (`sessionID`),
  KEY `movieID` (`movieID`),
  KEY `hallID` (`hallID`),
  CONSTRAINT `Sessions_ibfk_1` FOREIGN KEY (`movieID`) REFERENCES `Movies` (`movieID`),
  CONSTRAINT `Sessions_ibfk_2` FOREIGN KEY (`hallID`) REFERENCES `Halls` (`hallID`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Sessions`
--

LOCK TABLES `Sessions` WRITE;
/*!40000 ALTER TABLE `Sessions` DISABLE KEYS */;
INSERT INTO `Sessions` VALUES (1,1,'12:00:00','14:22:00','2021-05-01',1,10),(2,2,'12:00:00','14:55:00','2021-05-02',2,10),(3,3,'12:00:00','14:32:00','2021-05-03',3,10),(4,1,'15:00:00','17:22:00','2021-05-01',1,10),(5,2,'15:00:00','17:55:00','2021-05-02',2,10),(6,3,'15:00:00','17:32:00','2021-05-03',3,10),(7,1,'18:00:00','20:22:00','2021-05-01',1,10),(8,2,'18:00:00','20:55:00','2021-05-02',2,10),(9,3,'18:00:00','20:32:00','2021-05-03',3,10);
/*!40000 ALTER TABLE `Sessions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Tickets`
--

DROP TABLE IF EXISTS `Tickets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Tickets` (
  `ticketID` int NOT NULL AUTO_INCREMENT,
  `userID` int DEFAULT NULL,
  `sessionID` int DEFAULT NULL,
  `seatRow` int DEFAULT NULL,
  `paymentID` int DEFAULT NULL,
  `dateOfPurchase` date DEFAULT NULL,
  PRIMARY KEY (`ticketID`),
  KEY `userID` (`userID`),
  KEY `sessionID` (`sessionID`),
  KEY `paymentID` (`paymentID`),
  CONSTRAINT `Tickets_ibfk_1` FOREIGN KEY (`userID`) REFERENCES `Users` (`userID`),
  CONSTRAINT `Tickets_ibfk_2` FOREIGN KEY (`sessionID`) REFERENCES `Sessions` (`sessionID`),
  CONSTRAINT `Tickets_ibfk_3` FOREIGN KEY (`paymentID`) REFERENCES `Payments` (`paymentID`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Tickets`
--

LOCK TABLES `Tickets` WRITE;
/*!40000 ALTER TABLE `Tickets` DISABLE KEYS */;
INSERT INTO `Tickets` VALUES (1,2,1,1,1,'2021-05-01'),(2,3,2,2,2,'2021-05-02'),(3,4,3,3,3,'2021-05-03'),(4,2,1,2,1,'2021-05-01'),(5,3,2,3,2,'2021-05-02'),(6,4,3,4,3,'2021-05-03'),(7,2,1,3,1,'2021-05-01'),(8,3,2,4,2,'2021-05-02'),(9,4,3,5,3,'2021-05-03');
/*!40000 ALTER TABLE `Tickets` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Users`
--

DROP TABLE IF EXISTS `Users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Users` (
  `userID` int NOT NULL AUTO_INCREMENT,
  `userName` varchar(255) DEFAULT NULL,
  `hashedPassword` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `role` int DEFAULT NULL,
  PRIMARY KEY (`userID`),
  KEY `role` (`role`),
  CONSTRAINT `Users_ibfk_1` FOREIGN KEY (`role`) REFERENCES `Roles` (`roleID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Users`
--

LOCK TABLES `Users` WRITE;
/*!40000 ALTER TABLE `Users` DISABLE KEYS */;
INSERT INTO `Users` VALUES (1,'admin1','admin111','admin1@mail',1),(2,'client1','client111','client1@mail',2),(3,'client3','client333','client3@mail',2),(4,'client2','client222','client2@mail',2),(5,'admin2','admin222','admin2@mail',1);
/*!40000 ALTER TABLE `Users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-01-22 14:43:44
