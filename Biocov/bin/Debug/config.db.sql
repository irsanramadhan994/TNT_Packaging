BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "config" (
	"user"	TEXT NOT NULL,
	"ipprinter"	TEXT NOT NULL,
	"portprinter"	TEXT NOT NULL,
	"ipdb"	TEXT NOT NULL,
	"dbname"	TEXT NOT NULL,
	"hidscanner"	TEXT NOT NULL,
	"username_db"	TEXT NOT NULL,
	"password_db"	TEXT NOT NULL,
	"ipprinter2"	TEXT NOT NULL,
	"portprinter2"	TEXT NOT NULL,
	"ipprinter3"	TEXT NOT NULL,
	"portprinter3"	TEXT NOT NULL,
	"user_id"	INTEGER NOT NULL,
	PRIMARY KEY("user_id" AUTOINCREMENT)
);
INSERT INTO "config" VALUES ('kokom','84.16.118.43','6101','172.16.160.28','packaging_bt','15BAFD26','Sa','Biofarma2020#','84.16.118.44','6102','84.16.118.45','6103',1);
COMMIT;
