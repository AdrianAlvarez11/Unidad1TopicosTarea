using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Unidad1TopicosTarea
{
    public class Venta
    {
        public ObservableCollection<Pan> ListaPanes { get; set; } = new()
        {
            new Pan{Nombre="Concha",Precio=5m, UrlImagen="https://www.panovo.mx/wp-content/uploads/2022/03/conchas-congelacion.png"},
            new Pan{Nombre="Quequito", Precio=5m, UrlImagen="https://static.vecteezy.com/system/resources/previews/025/222/181/original/chocolate-chip-muffin-cake-isolated-on-transparent-background-png.png"},
            new Pan{Nombre="Dona", Precio=8m, UrlImagen="https://i.pinimg.com/originals/bd/07/23/bd072353f3c94f4430ae177b5e214309.png" },
            new Pan{Nombre="Rol", Precio=10m, UrlImagen="https://gocodough.com/wp-content/uploads/2021/06/donut-cinnamonroll.png" },
            new Pan{Nombre="Polvoron", Precio=6m, UrlImagen="https://images.squarespace-cdn.com/content/v1/5554e084e4b0734af02a1833/1431658784572-BB6Z37QJMQ7QACZZABFC/polovoron+amarillo.png"},
            new Pan{Nombre="Bolillo", Precio=1.50m, UrlImagen="https://i.pinimg.com/originals/04/4e/a5/044ea550b1faa4d6ac1cafb2dc70bd52.png" }
        };

        public ObservableCollection<Pan> ListaVenta { get; set; } = new();

        public ICommand AgregarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand ReiniciarCommand { get; set; }
        public decimal TotalPagar => ListaVenta.Sum(x => x.CantidadVenta * x.Precio);
        public byte CantidadAgregar { get; set; } = 1;


        public Venta()
        {
            Cargar();
            AgregarCommand = new RelayCommand<Pan>(AgregarPan);
            EliminarCommand = new RelayCommand<Pan>(EliminarPan);
            ReiniciarCommand = new RelayCommand(ReiniciarVenta);

        }
        public void AgregarPan(Pan? p)
        {

            if (p != null)
            {
                Pan? panGuardado = ListaVenta.FirstOrDefault(x => x.Nombre == p.Nombre);
                if (panGuardado == null)
                {
                    p.CantidadVenta = CantidadAgregar;
                    ListaVenta.Add(p);
                }
                if (panGuardado != null && (CantidadAgregar + panGuardado.CantidadVenta) > panGuardado.CantidadDisp)
                {
                    panGuardado.CantidadVenta = panGuardado.CantidadDisp;
                }
                if (panGuardado != null && panGuardado.CantidadVenta < panGuardado.CantidadDisp)
                {
                    panGuardado.CantidadVenta += CantidadAgregar;
                }

                PropertyChanged?.Invoke(this, new(nameof(TotalPagar)));
                PropertyChanged?.Invoke(ListaVenta, new(nameof(panGuardado.CantidadVenta)));
                Guardar();

            }

        }

        public void EliminarPan(Pan? p)
        {
            if (p != null)
            {
                if (p.CantidadVenta > 1 && p.CantidadVenta > CantidadAgregar)
                {
                    p.CantidadVenta -= CantidadAgregar;
                }
                else
                {
                    ListaVenta.Remove(p);
                }
                PropertyChanged?.Invoke(this, new(nameof(TotalPagar)));
                PropertyChanged?.Invoke(ListaVenta, new(nameof(p.CantidadVenta)));

                Guardar();
            }

        }
        public void ReiniciarVenta()
        {
            foreach (var pan in ListaPanes)
            {
                pan.CantidadVenta = 1;
            }
            ListaVenta.Clear();
            PropertyChanged?.Invoke(this, new(nameof(TotalPagar)));
            PropertyChanged?.Invoke(this, new(nameof(ListaVenta)));
            Guardar();
        }

        public void Guardar()
        {
            var json = JsonSerializer.Serialize(ListaVenta);
            File.WriteAllText("venta.json", json);
        }
        public void Cargar()
        {
            if (File.Exists("venta.json"))
            {
                var json = File.ReadAllText("venta.json");
                var datos = JsonSerializer.Deserialize<ObservableCollection<Pan>>(json);
                if (datos != null)
                {
                    ListaVenta = datos;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
