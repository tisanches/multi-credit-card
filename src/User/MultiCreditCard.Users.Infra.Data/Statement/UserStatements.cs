﻿namespace MultiCreditCard.Users.Infra.Data.Statement
{
    public static class UserStatements
    {
        public const string GetUserByUserId = @"SELECT UserId FROM USERS WHERE UserId = @userId";
        public const string GetUserByEmail = @"SELECT u.UserId, u.Email EletronicAddress FROM USERS u WHERE u.EMAIL = @email";
        public const string Create = @"
                                        INSERT INTO USERS(
	                                        UserId
	                                        ,UserName
	                                        ,DocumentNumber
	                                        ,Email
	                                        ,Password
	                                        ,CreationDate
                                        )
                                        VALUES(
	                                        @UserId
	                                        ,@UserName
	                                        ,@DocumentNumber
	                                        ,@Email
	                                        ,@Password
	                                        ,@CreationDate
                                        )";
    }
}
