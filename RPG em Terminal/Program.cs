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
    Inventario = new List<Item>
    {
        new Pocao { Nome = "Poção Pequena", QuantidadeCura = 30 },
        new Equipamento { Nome = "Espada Simples", Tipo = TipoEquipamento.Arma, BonusAtaque = 3 },
        new Equipamento { Nome = "Armadura de Couro", Tipo = TipoEquipamento.Armadura, BonusDefesa = 2 }
    }
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
        Console.WriteLine("5. Equipamentos");
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
            case "5":
                acaoEscolhida = AcaoCombate.GerenciarEquipamentos;
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

        if (acaoEscolhida == AcaoCombate.GerenciarEquipamentos)
        {
            GerenciarEquipamentos(heroi);
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

void GerenciarEquipamentos(Heroi heroi)
{
    Console.WriteLine($"\nAtaque: {heroi.Ataque} | Defesa: {heroi.Defesa}");
    Console.WriteLine($"Arma equipada: {heroi.ArmaEquipada?.Nome ?? "nenhuma"}");
    Console.WriteLine($"Armadura equipada: {heroi.ArmaduraEquipada?.Nome ?? "nenhuma"}");

    List<Equipamento> equipamentos = heroi.Inventario.OfType<Equipamento>().ToList();
    if (equipamentos.Count == 0)
    {
        Console.WriteLine("Você não possui equipamentos no inventário.");
        return;
    }

    Console.WriteLine("Equipamentos disponíveis:");
    for (int i = 0; i < equipamentos.Count; i++)
    {
        Equipamento equipamento = equipamentos[i];
        string bonus = equipamento.Tipo == TipoEquipamento.Arma
            ? $"+{equipamento.BonusAtaque} Ataque"
            : $"+{equipamento.BonusDefesa} Defesa";
        Console.WriteLine($"{i + 1}. {equipamento.Nome} ({bonus})");
    }
    Console.WriteLine("0. Desequipar arma");
    Console.WriteLine("-1. Desequipar armadura");
    Console.Write("Escolha um equipamento ou opção: ");

    if (!int.TryParse(Console.ReadLine(), out int escolha))
    {
        Console.WriteLine("Opção inválida!");
        return;
    }

    if (escolha == 0)
    {
        heroi.Desequipar(TipoEquipamento.Arma);
    }
    else if (escolha == -1)
    {
        heroi.Desequipar(TipoEquipamento.Armadura);
    }
    else if (escolha > 0 && escolha <= equipamentos.Count)
    {
        if (heroi.Equipar(equipamentos[escolha - 1]))
        {
            Console.WriteLine($"{equipamentos[escolha - 1].Nome} equipado!");
        }
    }
    else
    {
        Console.WriteLine("Opção inválida!");
    }
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

public enum TipoEquipamento
{
    Arma,
    Armadura
}

public class Equipamento : Item
{
    public TipoEquipamento Tipo { get; set; }
    public int BonusAtaque { get; set; }
    public int BonusDefesa { get; set; }
}

public class Heroi : Personagem
{
    public int Experiencia { get; set; } = 0;
    public int Nivel { get; set; } = 1;
    public List<Item> Inventario { get; set; } = new List<Item>();
    public Equipamento? ArmaEquipada { get; private set; }
    public Equipamento? ArmaduraEquipada { get; private set; }

    public int ExperienciaProximoNivel => (int)(100 * Math.Pow(1.5, Nivel - 1));

    public bool Equipar(Item item)
    {
        if (item is not Equipamento equipamento || !Inventario.Contains(item))
        {
            Console.WriteLine("Esse item não pode ser equipado.");
            return false;
        }

        Equipamento? equipamentoAnterior = equipamento.Tipo switch
        {
            TipoEquipamento.Arma => ArmaEquipada,
            TipoEquipamento.Armadura => ArmaduraEquipada,
            _ => null
        };

        if (equipamentoAnterior == equipamento)
        {
            Console.WriteLine("Esse equipamento já está equipado.");
            return false;
        }

        if (equipamentoAnterior != null)
        {
            AplicarBonus(equipamentoAnterior, -1);
            Inventario.Add(equipamentoAnterior);
        }

        Inventario.Remove(item);
        AplicarBonus(equipamento, 1);

        if (equipamento.Tipo == TipoEquipamento.Arma)
        {
            ArmaEquipada = equipamento;
        }
        else
        {
            ArmaduraEquipada = equipamento;
        }

        return true;
    }

    public bool Desequipar(TipoEquipamento tipo)
    {
        Equipamento? equipamento = tipo == TipoEquipamento.Arma ? ArmaEquipada : ArmaduraEquipada;
        if (equipamento == null)
        {
            Console.WriteLine("Não há equipamento desse tipo para desequipar.");
            return false;
        }

        AplicarBonus(equipamento, -1);
        Inventario.Add(equipamento);
        if (tipo == TipoEquipamento.Arma)
        {
            ArmaEquipada = null;
        }
        else
        {
            ArmaduraEquipada = null;
        }

        Console.WriteLine($"{equipamento.Nome} foi desequipado.");
        return true;
    }

    private void AplicarBonus(Equipamento equipamento, int fator)
    {
        Ataque += equipamento.BonusAtaque * fator;
        Defesa += equipamento.BonusDefesa * fator;
    }

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
    UsarItem,
    GerenciarEquipamentos
}

enum ResultadoCombate
{
    Vitoria,
    Derrota,
    Fuga
}
