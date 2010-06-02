CREATE TABLE LikesTable (id  integer, user_id TEXT, like_text TEXT, primary key (id));
CREATE TABLE DislikesTable (id  integer, user_id TEXT, dislike_text TEXT, primary key (id));
CREATE TABLE UserTable (id  integer, user_id TEXT, first_name TEXT, last_name TEXT, user_pic BLOB, gender TEXT, loc_city TEXT, loc_zipcode TEXT, loc_country TEXT, dob DATETIME, profile TEXT, primary key (id));
CREATE  TABLE FriendTable (user_id int, friend_id int);
