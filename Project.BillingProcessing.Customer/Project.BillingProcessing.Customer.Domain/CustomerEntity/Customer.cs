

namespace Project.BillingProcessing.Customer.Domain.CustomerEntity;
public class Customer : Entity
{
    public string Name { get; set; }
    public string State { get; set; }
    public long Identification { get; set; }

    public Customer(string name, string state, string identification)
    {
        this.Name = name;
        this.State = state;
        this.Identification = FormatIdentification(identification);
        this.DateCreation = DateTime.Now;

    }
    public Customer()
    {

    }
    public long FormatIdentification(string identifiation)
    {
        if (string.IsNullOrEmpty(identifiation))
            throw new CustomerException("Cpf invalido!");

        string valor = identifiation.Replace(".", "");
        valor = valor.Replace("-", "");
        if (valor.Length != 11)
            throw new CustomerException("Cpf invalido!");
        bool igual = true;
        for (int i = 1; i < 11 && igual; i++)
            if (valor[i] != valor[0])
                igual = false;
        if (igual || valor == "12345678909")
            throw new CustomerException("Cpf invalido!");
        int[] numeros = new int[11];
        for (int i = 0; i < 11; i++)
            numeros[i] = int.Parse(valor[i].ToString());
        int soma = 0;
        for (int i = 0; i < 9; i++)
            soma += (10 - i) * numeros[i];
        int resultado = soma % 11;
        if (resultado == 1 || resultado == 0)
            if (numeros[9] != 0)
                throw new CustomerException("Cpf invalido!");
            else if (numeros[9] != 11 - resultado)
                throw new CustomerException("Cpf invalido!");
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += (11 - i) * numeros[i];
        resultado = soma % 11;
        if (resultado == 1 || resultado == 0)
            if (numeros[10] != 0)
                throw new CustomerException("Cpf invalido!");
            else
            if (numeros[10] != 11 - resultado)
                throw new CustomerException("Cpf invalido!");

        return Convert.ToInt64(valor);
    }

}

