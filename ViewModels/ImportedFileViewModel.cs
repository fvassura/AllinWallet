﻿namespace AllinWallet.ViewModels
{
    public enum TipoFile
    {
        Csv,
        Pdf
    }


    public class ImportedFileViewModel
    {
        public TipoFile Tipo { get; set; }
        public DateTime DataImport { get; set; }
        public string Nome { get; set; }
        public bool Elaborato { get; set; }
        public string OutputFile { get; set; }

    }
}