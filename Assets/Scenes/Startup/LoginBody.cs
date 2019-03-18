using System;

public class LoginBody
{
    public string accessToken;
    public LoginBody(string at)
    {
        this.accessToken = at;
    }
}