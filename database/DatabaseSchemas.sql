CREATE  TABLE User("id" Integer PRIMARY KEY  NOT NULL , "name" VARCHAR, "profile" clob);
CREATE TABLE Comment("id" Integer PRIMARY KEY NOT NULL, "commenter" Integer, "user_commented_on" Integer, "content" VARCHAR);
