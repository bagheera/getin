CREATE TABLE Comment("id" Integer PRIMARY KEY NOT NULL, "commenter" Integer, "user_commented_on" Integer, "content" VARCHAR);
CREATE TABLE LikesTable (id  integer, user_id TEXT, like_text TEXT, primary key (id));
CREATE TABLE DislikesTable (id  integer, user_id TEXT, dislike_text TEXT, primary key (id));
CREATE TABLE UserTable (id  integer, user_id TEXT, first_name TEXT, last_name TEXT, user_pic BLOB, gender TEXT, loc_city TEXT, loc_zipcode TEXT, loc_country TEXT, dob DATETIME, profile TEXT, primary key (id));
CREATE  TABLE InvitationTable (user_id int, friend_id int);
CREATE  TABLE FriendTable (user_id int, friend_id int);
CREATE TABLE GroupTable(id  integer, name TEXT, desc TEXT);
CREATE  TABLE GroupMembershipTable (user_id int, id int);