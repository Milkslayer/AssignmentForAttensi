CREATE DATABASE testDB;

CREATE TABLE player (
    player_id INT IDENTITY(1, 1) PRIMARY KEY,
    name NVARCHAR(50) NOT NULL, 
    CONSTRAINT ak_player_name UNIQUE(name)  
);

CREATE TABLE player_stats (
    stat_id INT IDENTITY(1, 1) PRIMARY KEY,
    player_id INT NOT NULL,
    score INT NOT NULL,
    session_start DATETIME NOT NULL,
    session_end DATETIME NOT NULL,
    CONSTRAINT fk_player_id FOREIGN KEY (player_id) references player(player_id)
)

INSERT INTO player VALUES('Ole');
INSERT INTO player VALUES('Øyvind');
INSERT INTO player VALUES('Muhammed');
INSERT INTO player VALUES('hullabalooexposure');
INSERT INTO player VALUES('gossipsolid');
INSERT INTO player VALUES('bombadilquietus');
INSERT INTO player VALUES('forrestmotivated');
INSERT INTO player VALUES('graymarshpreach');
INSERT INTO player VALUES('shongmackin');
INSERT INTO player VALUES('descendomoronic');
INSERT INTO player VALUES('phonevigilant');
INSERT INTO player VALUES('huornlight');
INSERT INTO player VALUES('savageryphony');
INSERT INTO player VALUES('entertainmenacing');
INSERT INTO player VALUES('crochetsitchen');
INSERT INTO player VALUES('daisyravager');
INSERT INTO player VALUES('offendjust');
INSERT INTO player VALUES('nootedsurvival');
INSERT INTO player VALUES('wabbitmag');
INSERT INTO player VALUES('jaguarenunciate');