using System;


namespace AccountingLib.Security
{
    public interface ILogin
    {
        int GetId();
        String GetUsername();
        String GetPassword();
    }

}
