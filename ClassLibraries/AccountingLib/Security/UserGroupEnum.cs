using System;
using DocMageFramework.CustomAttributes;


namespace AccountingLib.Security
{
    public enum UserGroupEnum
    {
        [AssociatedText("Administradores")]
        Administrators,
        [AssociatedText("Colaboradores/Funcionários")]
        CompanyMembers,
    }

}
