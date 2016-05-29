using System;


namespace DocMageFramework.AppUtils
{
    /// <summary>
    /// Interface para troca de mensagens (entre dois objetos), objetivando diminuir o acoplamento
    /// Disponível também no instalador ( duplicação proposital de linhas de código, não refatorar )
    /// </summary>
    public interface IListener
    {
        void NotifyObject(Object obj);
    }

}
