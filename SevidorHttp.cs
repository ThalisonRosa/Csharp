using System.Net;
using System.Net.Sockets;
using System.Text;
    
    public class Servidor{

    private TcpListener Controlador {get; set;}
    private int Porta {get;set;}
    private int QtdRequ {get;set;}
    public object Econding { get; private set; }

    public Servidor(int porta = 8080){
        this.Porta = porta;

        try
        {
            this.Controlador = new TcpListener(IPAddress.Parse("127.0.0.1"), this.Porta);
            this.Controlador.Start();
            Console.WriteLine($"O servidor está rodando na porta {this.Porta}");
            Console.WriteLine($"Para acessar digite em seu navegado http://localhost:{this.Porta}");

            Task servidorHttpTask = Task.Run(() => ContadorRequ());
            servidorHttpTask.GetAwaiter().GetResult();
        }
        catch (Exception e)
        {
            
            Console.WriteLine($"Erro ao iniciar o servidor na porta {this.Porta}");
        }
    }

    private async Task ContadorRequ(){
        while(true){

            Socket conexao = await this.Controlador.AcceptSocketAsync();
            this.QtdRequ ++;
            Task task = Task.Run(()=> ProcessarRequest (conexao, this.QtdRequ));
        }
    }
    private void ProcessarRequest(Socket conexao , int numeroRequest){
        Console.WriteLine($"Processando request #{numeroRequest}");
        if(conexao.Connected){
           var utf8 = Encoding.UTF8;
            byte[]  byteRequisicao = new byte[1024];
            conexao.Receive(byteRequisicao, byteRequisicao.Length, 0);
            string textoRequisicao = utf8.GetString(byteRequisicao,byteRequisicao.Length, 0).Replace((char)0,' ').Trim();
            if(textoRequisicao.Length > 0){
                    Console.WriteLine($"\n {textoRequisicao}");
                    conexao.Close();
            }
        }
            Console.WriteLine($"o {numeroRequest} foi finalizado");
    }


}
