CREATE TABLE `aspnet_application` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `LoweredName` varchar(255) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Application_UC1` (`LoweredName`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;



CREATE TABLE `aspnet_membershipuser` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `LoweredName` varchar(255) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Password` varchar(128) NOT NULL,
  `PasswordFormat` int(11) NOT NULL,
  `PasswordSalt` varchar(128) NOT NULL,
  `Email` varchar(128) NOT NULL,
  `LoweredEmail` varchar(128) NOT NULL,
  `PasswordQuestion` varchar(255) DEFAULT NULL,
  `PasswordAnswer` varchar(255) DEFAULT NULL,
  `Comments` varchar(3000) DEFAULT NULL,
  `IsApproved` bit(1) NOT NULL,
  `IsLockedOut` bit(1) NOT NULL,
  `CreationDate` datetime NOT NULL,
  `LastActivityDate` datetime NOT NULL,
  `LastLoginDate` datetime NOT NULL,
  `LastLockedOutDate` datetime NOT NULL,
  `LastPasswordChangeDate` datetime NOT NULL,
  `FailedPwdAttemptCnt` int(11) NOT NULL,
  `FailedPwdAttemptWndStart` datetime NOT NULL,
  `FailedPwdAnsAttemptCnt` int(11) NOT NULL,
  `FailedPwdAnsAttemptWndStart` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `MembershipUser_UC1` (`LoweredName`),
  KEY `MembershipUser_IDX2` (`LastActivityDate`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1;


CREATE TABLE `aspnet_role` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `LoweredName` varchar(255) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Role_UC1` (`LoweredName`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;



CREATE TABLE `aspnet_actions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ControllerName` varchar(255) NOT NULL,
  `ActionName` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8;


CREATE TABLE `aspnet_applicationuser` (
  `ApplicationId` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  PRIMARY KEY (`ApplicationId`,`UserId`),
  KEY `User_AppUser_FK1` (`UserId`),
  CONSTRAINT `App_AppUser_FK1` FOREIGN KEY (`ApplicationId`) REFERENCES `aspnet_application` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `User_AppUser_FK1` FOREIGN KEY (`UserId`) REFERENCES `aspnet_membershipuser` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


CREATE TABLE `aspnet_applicationrole` (
  `ApplicationId` int(11) NOT NULL,
  `RoleId` int(11) NOT NULL,
  PRIMARY KEY (`ApplicationId`,`RoleId`),
  KEY `ApplicationRole_IDX1` (`RoleId`),
  CONSTRAINT `App_AppRole_FK1` FOREIGN KEY (`ApplicationId`) REFERENCES `aspnet_application` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `Role_AppRole_FK1` FOREIGN KEY (`RoleId`) REFERENCES `aspnet_role` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `aspnet_applicationuserrole` (
  `ApplicationId` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `RoleId` int(11) NOT NULL,
  PRIMARY KEY (`ApplicationId`,`UserId`,`RoleId`),
  KEY `Role_AppUserRole_FK1` (`RoleId`),
  KEY `User_AppUserRole_FK1` (`UserId`),
  CONSTRAINT `App_AppUserRole_FK1` FOREIGN KEY (`ApplicationId`) REFERENCES `aspnet_application` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `Role_AppUserRole_FK1` FOREIGN KEY (`RoleId`) REFERENCES `aspnet_role` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `User_AppUserRole_FK1` FOREIGN KEY (`UserId`) REFERENCES `aspnet_membershipuser` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


CREATE TABLE `aspnet_roleactions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RoleId` int(11) DEFAULT NULL,
  `UserId` int(11) DEFAULT NULL,
  `ActionId` int(11) NOT NULL,
  `PermissionType` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_roleaction_action` (`ActionId`),
  KEY `fk_roleaction_role` (`RoleId`),
  CONSTRAINT `fk_roleaction_action` FOREIGN KEY (`ActionId`) REFERENCES `aspnet_actions` (`Id`),
  CONSTRAINT `fk_roleaction_role` FOREIGN KEY (`RoleId`) REFERENCES `aspnet_role` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8;



CREATE TABLE `aspnet_profiles` (
  `PropertyNames` longtext,
  `PropertyValuesString` longtext,
  `LastActivityDate` datetime DEFAULT NULL,
  `PropertyValuesBinary` longblob,
  `UserId` int(11) NOT NULL,
  PRIMARY KEY (`UserId`) USING BTREE,
  UNIQUE KEY `Index_3` (`UserId`),
  CONSTRAINT `FK_profiles_1` FOREIGN KEY (`UserId`) REFERENCES `aspnet_membershipuser` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;



CREATE TABLE `system_log` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Date` datetime DEFAULT NULL,
  `Thread` varchar(255) DEFAULT NULL,
  `Level` varchar(50) DEFAULT NULL,
  `Logger` varchar(255) DEFAULT NULL,
  `Message` varchar(4000) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7734 DEFAULT CHARSET=utf8;

