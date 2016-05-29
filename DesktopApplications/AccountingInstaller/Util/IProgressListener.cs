using System;


namespace AccountingInstaller.Util
{
    public interface IProgressListener
    {
        // Aviso quando é iniciada a medição de progresso
        void ProgressInitialized();
        // Aviso quando houve mudança no nível de progresso, passa o a porcentagem concluída
        void ProgressChanged(int currentProgress);
        // Aviso quando é concluída a medição de progresso
        void ProgressConcluded();
    }

}
