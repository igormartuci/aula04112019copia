﻿using OficinaMVVM.Models;
using OficinaMVVM.Services.Atendimentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace OficinaMVVM.ViewModels.Atendimentos
{
    public class CRUDViewModel : BaseViewModel
    {
        private IAtendimentoService cService = new AtendimentoService();
        private Atendimento Atendimento { get; set; }
        public ICommand GravarCommand { get; set; }

        public ICommand PesquisarCommand { get; set; }

        public string ClienteNome
        {
            get
            {
                return this.Atendimento.Cliente == null ? "Localize o cliente" : this.Atendimento.Cliente.Nome;
            }
        }
        public CRUDViewModel(Atendimento Atendimento)
        {
            this.Atendimento = Atendimento;
            RegistarCommands();
        }

        public void RegistarCommands()
        {
            PesquisarCommand = new Command(() =>
            {
                MessagingCenter.Send<Atendimento>(Atendimento, "MostrarPesquisaCliente");
            });

            GravarCommand = new Command(async () =>
            {
                if (Atendimento.AtendimentoID != null)
                    Atendimento.NotificarListView = true;
                Atendimento.ClienteID = Atendimento.Cliente.Id;
                await cService.PostAtendimentoAsync(Atendimento);
                MessagingCenter.Send<string>("Dados salvo com sucesso.", "InformacaoCRUD");
            },
            () =>
            {
                return ((this.Atendimento.Cliente != null) &&
                !string.IsNullOrEmpty(this.Atendimento.Cliente.Nome)
                && !string.IsNullOrEmpty(this.Atendimento.Veiculo)
                & (this.Atendimento.DataHoraPrometida > this.Atendimento.DataHoraChegada));
            });

        }
    
        public Cliente Cliente
        {
            get { return this.Atendimento.Cliente; }
            set
            {
                this.Atendimento.Cliente = value;
                OnPropertyChanged(nameof(ClienteNome));
                ((Command)GravarCommand).ChangeCanExecute();
            }
        }

        public string Veiculo
        {
            get { return this.Atendimento.Veiculo; }
            set
            {
                this.Atendimento.Veiculo = value;
                OnPropertyChanged();
                ((Command)GravarCommand).ChangeCanExecute();
            }
        }

        public DateTime DataChegada
        {
            get { return this.Atendimento.DataHoraChegada; }
            set
            {
                this.Atendimento.DataHoraChegada = new DateTime(value.Year, value.Month, value.Day,
                HoraChegada.Hours, HoraChegada.Minutes
                , 0);
                OnPropertyChanged();
                ((Command)GravarCommand).ChangeCanExecute();
            }
        }
        public TimeSpan HoraChegada
        {
            get
            {
                return new TimeSpan(this.Atendimento.DataHoraChegada.Hour,
                this.Atendimento.DataHoraChegada.Minute, 0);
            }
            set
            {
                this.Atendimento.DataHoraChegada = new DateTime(DataChegada.Year, DataChegada.Month,
                DataChegada.Day, value.Hours, value.Minutes, 0);
                OnPropertyChanged();
                ((Command)GravarCommand).ChangeCanExecute();
            }
        }

        public DateTime DataPrometida
        {
            get { return this.Atendimento.DataHoraPrometida; }
            set
            {
                this.Atendimento.DataHoraPrometida = new DateTime(value.Year, value.Month, value.Day,
                HoraPrometida.Hours, HoraPrometida.Minutes, 0);
                OnPropertyChanged();
                ((Command)GravarCommand).ChangeCanExecute();
            }

        }

        public TimeSpan HoraPrometida
        {
            get
            {
                return new TimeSpan(this.Atendimento.DataHoraPrometida.Hour,
                this.Atendimento.DataHoraPrometida.Minute, 0);
            }
            set
            {
                this.Atendimento.DataHoraPrometida = new DateTime(DataPrometida.Year,
                DataPrometida.Month, DataPrometida.Day,
                value.Hours, value.Minutes, 0);
                OnPropertyChanged();
                ((Command)GravarCommand).ChangeCanExecute();
            }
        }


                
    }
}
