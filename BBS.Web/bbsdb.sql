-- --------------------------------------------------------
-- 主机:                           127.0.0.1
-- 服务器版本:                        5.7.28-log - MySQL Community Server (GPL)
-- 服务器OS:                        Win64
-- HeidiSQL 版本:                  10.2.0.5599
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for bbsdb
CREATE DATABASE IF NOT EXISTS `bbsdb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_bin */;
USE `bbsdb`;

-- Dumping structure for table bbsdb.account
CREATE TABLE IF NOT EXISTS `account` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Account` varchar(30) COLLATE utf8mb4_bin NOT NULL COMMENT '账号',
  `Password` varchar(50) COLLATE utf8mb4_bin NOT NULL COMMENT '密码',
  `NickName` varchar(40) COLLATE utf8mb4_bin NOT NULL DEFAULT '' COMMENT '昵称',
  `Avatar` varchar(255) COLLATE utf8mb4_bin NOT NULL DEFAULT '' COMMENT '头像地址',
  `Integral` int(11) NOT NULL DEFAULT '0' COMMENT '积分',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Account` (`Account`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin COMMENT='用户表';

-- Data exporting was unselected.

-- Dumping structure for table bbsdb.admin
CREATE TABLE IF NOT EXISTS `admin` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Account` varchar(20) COLLATE utf8mb4_bin NOT NULL DEFAULT '' COMMENT '账号',
  `NickName` varchar(30) COLLATE utf8mb4_bin NOT NULL DEFAULT '' COMMENT '昵称',
  `Password` varchar(50) COLLATE utf8mb4_bin NOT NULL DEFAULT '' COMMENT '密码',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin COMMENT='后台账号表';

-- Data exporting was unselected.

-- Dumping structure for table bbsdb.catalog
CREATE TABLE IF NOT EXISTS `catalog` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(50) COLLATE utf8mb4_bin NOT NULL COMMENT '名称',
  `Description` varchar(800) COLLATE utf8mb4_bin NOT NULL COMMENT '描述',
  `Cover` varchar(500) COLLATE utf8mb4_bin NOT NULL DEFAULT '' COMMENT '封面图片',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin COMMENT='板块表';

-- Data exporting was unselected.

-- Dumping structure for table bbsdb.comment
CREATE TABLE IF NOT EXISTS `comment` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TopicId` int(11) NOT NULL COMMENT '主题ID',
  `AccountId` int(11) NOT NULL COMMENT '用户ID',
  `QuoteAccountId` int(11) NOT NULL COMMENT '被引用人ID',
  `Contents` varchar(2000) COLLATE utf8mb4_bin NOT NULL COMMENT '回复内容',
  `ThumbsUpCount` int(11) NOT NULL DEFAULT '0' COMMENT '赞数',
  `ThumbsDownCount` int(11) NOT NULL DEFAULT '0' COMMENT '踩数',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin COMMENT='评论表';

-- Data exporting was unselected.

-- Dumping structure for table bbsdb.favorite
CREATE TABLE IF NOT EXISTS `favorite` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ItemId` int(11) NOT NULL COMMENT '被收藏项ID',
  `AccountId` int(11) NOT NULL COMMENT '所属用户ID',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ItemId_AccountId` (`ItemId`,`AccountId`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin COMMENT='收藏表';

-- Data exporting was unselected.

-- Dumping structure for table bbsdb.feedback
CREATE TABLE IF NOT EXISTS `feedback` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(20) COLLATE utf8mb4_bin NOT NULL COMMENT '名字',
  `Contact` varchar(40) COLLATE utf8mb4_bin NOT NULL COMMENT '联系方式',
  `Contents` varchar(500) COLLATE utf8mb4_bin NOT NULL COMMENT '反馈内容',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin COMMENT='反馈表';

-- Data exporting was unselected.

-- Dumping structure for table bbsdb.follow
CREATE TABLE IF NOT EXISTS `follow` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AccountId` int(11) NOT NULL COMMENT '账号ID',
  `FollowAccountId` int(11) NOT NULL COMMENT '被关注者ID',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `AccountId_FollowAccountId` (`AccountId`,`FollowAccountId`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin COMMENT='粉丝表';

-- Data exporting was unselected.

-- Dumping structure for table bbsdb.tag
CREATE TABLE IF NOT EXISTS `tag` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TagName` varchar(40) COLLATE utf8mb4_bin NOT NULL COMMENT '标签名称',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin COMMENT='标签表';

-- Data exporting was unselected.

-- Dumping structure for table bbsdb.thumbs_up
CREATE TABLE IF NOT EXISTS `thumbs_up` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ItemId` int(11) NOT NULL COMMENT '被赞项ID',
  `IsThumb` bit(1) NOT NULL COMMENT '1：赞，0：踩',
  `AccountId` int(11) NOT NULL COMMENT '所属用户ID',
  `ItemType` int(11) NOT NULL COMMENT '点赞项类型2：主题，4:评论',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `ItemId_IsThumb_AccountId_ItemType` (`ItemId`,`IsThumb`,`AccountId`,`ItemType`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin COMMENT='点赞表';

-- Data exporting was unselected.

-- Dumping structure for table bbsdb.topic
CREATE TABLE IF NOT EXISTS `topic` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(150) COLLATE utf8mb4_bin NOT NULL COMMENT '标题',
  `AccountId` int(11) NOT NULL COMMENT '用户ID',
  `CatalogId` int(11) NOT NULL COMMENT '话题ID',
  `Contents` text COLLATE utf8mb4_bin NOT NULL COMMENT '内容',
  `ThumbsUpCount` int(11) NOT NULL DEFAULT '0' COMMENT '赞数',
  `ThumbsDownCount` int(11) NOT NULL DEFAULT '0' COMMENT '踩数',
  `TrailCount` int(11) NOT NULL DEFAULT '0' COMMENT '浏览数',
  `CreateTime` datetime NOT NULL,
  `LastUpdateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin COMMENT='话题表';

-- Data exporting was unselected.

-- Dumping structure for table bbsdb.topic_tag
CREATE TABLE IF NOT EXISTS `topic_tag` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TopicId` int(11) NOT NULL COMMENT '主题ID',
  `TagId` int(11) NOT NULL COMMENT '标签ID',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin COMMENT='主题和标签关系表';

-- Data exporting was unselected.

-- Dumping structure for table bbsdb.trail
CREATE TABLE IF NOT EXISTS `trail` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AccountId` int(11) NOT NULL COMMENT '当前登录人ID',
  `ItemId` int(11) NOT NULL COMMENT '被浏览项ID',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin COMMENT='浏览记录表';

-- Data exporting was unselected.

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;

DELETE FROM `admin`;
/*!40000 ALTER TABLE `admin` DISABLE KEYS */;
INSERT INTO `admin` (`Id`, `Account`, `NickName`, `Password`, `CreateTime`) VALUES
	(1, 'admin', '管理员', '378F730224418319ADDCCEA85318BFA3', SYSDATE());
