using System;

Random random = new Random();

bool fugiuComSucesso = false;

var heroi = new Heroi
{
    Nome = "Davi",
    PontosDeVida = 100,
    Ataque = 15,
    Defesa = 5,
    PontosDeVidaMaximo = 100,
    Inventario = new List<Item> { new Pocao { Nome = "Poção Pequena", QuantidadeCura = 30} }
};

List<Inimigo> inimigos = new List<Inimigo>
{
    new Inimigo { Nome = "Goblin", PontosDeVida = 50, Ataque = 10, Defesa = 2, RecompensaXP = 20 },
    new Inimigo { Nome = "Orc", PontosDeVida = 80, Ataque = 15, Defesa = 5, RecompensaXP = 40 },
    new Inimigo { Nome = "Troll", PontosDeVida = 120, Ataque = 20, Defesa = 8, RecompensaXP = 60 },
    new Inimigo { Nome = "Dragão", PontosDeVida = 200, Ataque = 30, Defesa = 15, RecompensaXP = 100 }
};

foreach (var inimigoAtual in inimigos)
{
    Console.WriteLine($"\nUm {inimigoAtual.Nome} apareceu! ({heroi.Nome} entra com {heroi.PontosDeVida} de vida)");

    ResultadoCombate resultado = Combater(heroi, inimigoAtual);

    if (resultado == ResultadoCombate.Fuga)
    {
        fugiuComSucesso = true;
        break;
    }
    else if (resultado == ResultadoCombate.Derrota)
    {
        Console.WriteLine($"\n{heroi.Nome} foi derrotado! Fim de jogo.");
        return;
    }
}

if (fugiuComSucesso)
{
    Console.WriteLine($"\n{heroi.Nome} escapou da jornada!");
}
else
{
    Console.WriteLine($"\n{heroi.Nome} venceu todos os inimigos! Jornada finalizada!");
}

ResultadoCombate Combater(Heroi heroi, Inimigo inimigo)
{
    while (heroi.EstaVivo() && inimigo.EstaVivo())
    {
        Console.WriteLine();
        Console.WriteLine("O que você vai fazer?");
        Console.WriteLine("1. Atacar");
        Console.WriteLine("2. Defender");
        Console.WriteLine("3. Fugir");
        Console.WriteLine("4. Inventário");
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
            case "4":
                acaoEscolhida = AcaoCombate.UsarItem;
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
                heroi.GanharExperiencia(inimigo.RecompensaXP);
                return ResultadoCombate.Vitoria;
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
                return ResultadoCombate.Fuga;
            }
            else
            {
                Console.WriteLine($"{heroi.Nome} falhou ao tentar fugir!");
            }
        }

        if (acaoEscolhida == AcaoCombate.UsarItem)
        {
            if (heroi.Inventario.Count == 0)
            {
                Console.WriteLine("Inventário vazio!");
            }
            else
            {
                Console.WriteLine("Itens disponíveis:");
                for (int i = 0; i < heroi.Inventario.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {heroi.Inventario[i].Nome}");
                }
                Console.Write("Escolha um item para usar: ");
                string? itemEscolhido = Console.ReadLine();
                if (int.TryParse(itemEscolhido, out int indiceItem) && indiceItem > 0 && indiceItem <= heroi.Inventario.Count)
                {
                    Item item = heroi.Inventario[indiceItem - 1];
                    if (item is Pocao pocao)
                    {
                        heroi.PontosDeVida = Math.Min(heroi.PontosDeVida + pocao.QuantidadeCura, heroi.PontosDeVidaMaximo);
                        Console.WriteLine($"{heroi.Nome} usou {pocao.Nome} e recuperou {pocao.QuantidadeCura} de vida! (HP atual: {heroi.PontosDeVida})");
                        heroi.Inventario.RemoveAt(indiceItem - 1);
                    }
                    else
                    {
                        Console.WriteLine("Item não utilizável!");
                    }
                }
                else
                {
                    Console.WriteLine("Opção inválida!");
                }
            }
        }

        if (inimigo.EstaVivo())
        {
            int danoInimigo = random.Next(inimigo.Ataque - 2, inimigo.Ataque + 3);
            heroi.ReceberDano(danoInimigo);

            if (!heroi.EstaVivo())
            {
                Console.WriteLine($"\n{heroi.Nome} foi derrotado!");
                return ResultadoCombate.Derrota;
            }
        }

        if (heroiDefendendo)
        {
            heroi.Defesa -= 5;
        }
    }

    return heroi.EstaVivo() ? ResultadoCombate.Vitoria : ResultadoCombate.Derrota;
}

public abstract class Personagem
{
    public string Nome { get; set; } = string.Empty;
    public int PontosDeVida { get; set; }
    public int PontosDeVidaMaximo { get; set;  }
    public int Ataque { get; set; }
    public int Defesa { get; set; }

    public bool EstaVivo() => PontosDeVida > 0;

    public virtual void ReceberDano(int dano)
    {
        int danoReal = Math.Max(dano - Defesa, 0);
        PontosDeVida = Math.Max(PontosDeVida - danoReal, 0);
        Console.WriteLine($"{Nome} recebeu {danoReal} de dano! (HP restante: {PontosDeVida})");
    }
}

public abstract class Item
{
    public string Nome { get; set; } = string.Empty;
}

public class Pocao : Item
{
    public int QuantidadeCura { get; set; }
}

public class Heroi : Personagem
{
    public int Experiencia { get; set; } = 0;
    public int Nivel { get; set; } = 1;
    public List<Item> Inventario { get; set; } = new List<Item>();

    public int ExperienciaProximoNivel => (int)(100 * Math.Pow(1.5, Nivel - 1));

    public void GanharExperiencia(int xp)
    {
        Experiencia += xp;
        Console.WriteLine($"{Nome} ganhou {xp} de experiência! (XP total: {Experiencia})");
        while (Experiencia >= ExperienciaProximoNivel)
        {
            SubirDeNivel();
        }

        Console.WriteLine($"XP: {Experiencia}/{ExperienciaProximoNivel}");
    }

    private void SubirDeNivel()
    {
        int xpNecessario = ExperienciaProximoNivel;
        Experiencia -= xpNecessario;
        Nivel++;

        Ataque += 3;
        Defesa += 1;
        PontosDeVida += 20;
        PontosDeVidaMaximo += 20;

        Console.WriteLine($"{Nome} subiu para o nível {Nivel}!");
    }
}

public class Inimigo : Personagem
{
    public int RecompensaXP { get; set; }
}

enum AcaoCombate
{
    Atacar,
    Defender,
    Fugir,
    UsarItem
}

enum ResultadoCombate
{
    Vitoria,
    Derrota,
    Fuga
}