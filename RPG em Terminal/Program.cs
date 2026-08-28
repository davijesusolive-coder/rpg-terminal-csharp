using System;

Random random = new Random();

bool fugiuComSucesso = false;

var heroi = new Heroi
{
    Nome = "Davi",
    PontosDeVida = 100,
    Ataque = 15,
    Defesa = 5
};

var inimigo = new Inimigo
{
    Nome = "Goblin",
    PontosDeVida = 50,
    Ataque = 10,
    Defesa = 2,
    RecompensaXP = 20
};

Console.WriteLine($"Um {inimigo.Nome} selvagem apareceu!");

while (heroi.EstaVivo() && inimigo.EstaVivo())
{
    Console.WriteLine();
    Console.WriteLine("O que você vai fazer?");
    Console.WriteLine("1. Atacar");
    Console.WriteLine("2. Defender");
    Console.WriteLine("3. Fugir");
    Console.Write("Escolha uma opção: ");

    string? opcaoEscolhida = Console.ReadLine();
    AcaoCombate acaoEscolhida;

    switch (opcaoEscolhida)
    {
        case "1":
            acaoEscolhida = AcaoCombate.Atacar;
            break;
        case "2":
            acaoEscolhida = AcaoCombate.Defender;
            break;
        case "3":
            acaoEscolhida = AcaoCombate.Fugir;
            break;
        default:
            Console.WriteLine("Opção Inválida!");
            continue;
    }

    bool heroiDefendendo = false;

    if (acaoEscolhida == AcaoCombate.Atacar)
    {
        int dano = random.Next(heroi.Ataque - 3, heroi.Ataque + 4);
        inimigo.ReceberDano(dano);

        if (!inimigo.EstaVivo())
        {
            Console.WriteLine($"\n{inimigo.Nome} foi derrotado!");
            heroi.Experiencia += inimigo.RecompensaXP;
            Console.WriteLine($"{heroi.Nome} ganhou {inimigo.RecompensaXP} de experiência! (XP total: {heroi.Experiencia})");
            break;
        }
    }

    if (acaoEscolhida == AcaoCombate.Defender)
    {
        Console.WriteLine($"\n{heroi.Nome} está se defendendo!");
        heroi.Defesa += 5;
        heroiDefendendo = true;
    }

    if (acaoEscolhida == AcaoCombate.Fugir)
    {
        Console.WriteLine($"\n{heroi.Nome} tentou fugir!");
        if (random.Next(0, 2) == 0)
        {
            Console.WriteLine($"{heroi.Nome} conseguiu fugir com sucesso!");
            fugiuComSucesso = true;
            break;
        }
        else
        {
            Console.WriteLine($"{heroi.Nome} falhou ao tentar fugir!");
        }
    }

    if (inimigo.EstaVivo())
    {
        int danoInimigo = random.Next(inimigo.Ataque - 2, inimigo.Ataque + 3);
        heroi.ReceberDano(danoInimigo);

        if (!heroi.EstaVivo())
        {
            Console.WriteLine($"\n{heroi.Nome} foi derrotado!");
            break;
        }
    }

    if (heroiDefendendo)
    {
        heroi.Defesa -= 5;
    }

}

if (fugiuComSucesso)
{
    Console.WriteLine($"\n{heroi.Nome} escapou da batalha!");
}
else if (heroi.EstaVivo())
{
    Console.WriteLine($"\n{heroi.Nome} venceu a batalha!");
}
else
{
    Console.WriteLine($"\n{inimigo.Nome} venceu a batalha!");
}
public abstract class Personagem
{
    public string Nome { get; set; } = string.Empty;
    public int PontosDeVida { get; set; }
    public int Ataque { get; set; }
    public int Defesa { get; set; }

    public bool EstaVivo() => PontosDeVida > 0;

    public virtual void ReceberDano(int dano)
    {
        int danoReal = Math.Max(dano - Defesa, 0);
        PontosDeVida -= danoReal;
        Console.WriteLine($"{Nome} recebeu {danoReal} de dano! (HP restante: {PontosDeVida})");
    }
}

public class Heroi : Personagem
{
    public int Experiencia { get; set; } = 0;
}

public class Inimigo : Personagem
{
    public int RecompensaXP { get; set; }
}

enum AcaoCombate
{
    Atacar,
    Defender,
    Fugir
}
