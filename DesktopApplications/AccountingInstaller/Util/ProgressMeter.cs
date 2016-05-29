using System;


namespace AccountingInstaller.Util
{
    // Classe utilitária para medição de progresso na instalação
    public class ProgressMeter
    {
        private IProgressListener progressListener;

        private long currentAmount;

        private long totalAmount;


        public ProgressMeter(long totalAmount, IProgressListener progressListener)
        {
            this.totalAmount = totalAmount;
            this.progressListener = progressListener;

            // Corrige(seta o valor default) caso o valor fornecido seja inválido
            if (totalAmount <= 0) this.totalAmount = 100;

            progressListener.ProgressInitialized(); // Avisa o listener
        }

        public void IncreaseProgress(long amount)
        {
            // aborta caso o valor fornecido seja inválido
            if (amount <= 0) return;

            if ((currentAmount + amount) >= totalAmount)
            {
                progressListener.ProgressConcluded(); // Avisa o listener
                return;
            }

            currentAmount = currentAmount + amount;
            double ratio = (double)currentAmount / (double)totalAmount;
            progressListener.ProgressChanged((int)(ratio*100)); // Avisa o listener
        }
    }

}
