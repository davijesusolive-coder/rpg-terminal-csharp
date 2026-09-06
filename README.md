# RPG em Terminal (C#)

Um RPG por turnos, rodando inteiramente no console, construído como projeto de estudo de Orientação a Objetos em C#.

## Sobre o projeto

Este projeto foi desenvolvido como parte de um plano de estudos de C# e .NET, com foco em aplicar na prática conceitos de:

- Herança e polimorfismo (classes `Personagem`, `Heroi`, `Inimigo`, `Item`, `Pocao`)
- Classes abstratas (`abstract class Personagem`, `abstract class Item`)
- `enum` para representar ações de combate e resultados de combate
- `switch` para tratar as escolhas do jogador
- A classe `Random` para calcular dano variável e chance de fuga
- Propriedades calculadas (`=>`) para valores sempre recalculados, como a XP necessária pro próximo nível
- Encapsulamento com métodos `private` (detalhes internos) vs `public` (comportamento exposto)
- Extração de método (Extract Method) para reaproveitar a lógica de combate contra múltiplos inimigos
- Type pattern matching (`is Tipo variavel`) para trabalhar com itens polimórficos
- Boas práticas de Clean Code e SOLID aplicadas ao design das classes

## Como funciona

O jogo simula uma jornada de combates por turnos entre um Herói e uma sequência de Inimigos:

1. A cada turno, o jogador escolhe uma ação: **Atacar**, **Defender**, **Fugir** ou **Usar Item**
2. **Atacar** causa dano aleatório baseado no atributo de Ataque, descontando a Defesa do alvo
3. **Defender** aumenta temporariamente a Defesa do herói, reduzindo o dano do contra-ataque naquele turno
4. **Fugir** tem uma chance de 50% de encerrar o combate sem vencedor nem perdedor
5. **Usar Item** permite consumir uma poção do inventário para recuperar pontos de vida, sem ultrapassar o máximo
6. Se o inimigo sobreviver ao ataque, ele contra-ataca automaticamente
7. Ao derrotar um inimigo, o herói ganha experiência (XP) e pode subir de nível, aumentando seus atributos e vida máxima
8. O herói enfrenta os inimigos da lista **em sequência** — o dano é **cumulativo entre as batalhas** (não há cura automática), exigindo gerenciamento de risco
9. A jornada termina quando o herói derrota todos os inimigos, é derrotado, ou foge com sucesso

## Sistema de níveis

- Cada nível exige uma quantidade de XP para o próximo, com curva exponencial (`100 * 1.5^(Nivel-1)`)
- Ao subir de nível, o herói ganha bônus de Ataque, Defesa, Pontos de Vida e Pontos de Vida Máximo
- Suporta subir **múltiplos níveis de uma vez**, caso o XP ganho seja suficiente
- Mostra o progresso atual a cada ganho de experiência (`XP: 50/150`)

## Sistema de itens

- Itens consumíveis (**Poções**) curam vida, sem ultrapassar `PontosDeVidaMaximo`
- O inventário guarda itens de forma genérica (`List<Item>`), preparado para novos tipos de item no futuro (equipamentos)
- Poções são removidas do inventário após o uso (consumíveis)

## Estrutura das classes

```
Personagem (classe abstrata)
├── Nome, PontosDeVida, PontosDeVidaMaximo, Ataque, Defesa
├── EstaVivo()
├── ReceberDano(int dano)
│
├── Heroi : Personagem
│   ├── Experiencia, Nivel, Inventario (List<Item>)
│   ├── ExperienciaProximoNivel (propriedade calculada)
│   ├── GanharExperiencia(int xp)
│   └── SubirDeNivel() [privado]
│
└── Inimigo : Personagem
    └── RecompensaXP

Item (classe abstrata)
└── Pocao : Item
    └── QuantidadeCura

Combater(Heroi, Inimigo) -> ResultadoCombate
    └── enum ResultadoCombate { Vitoria, Derrota, Fuga }

enum AcaoCombate { Atacar, Defender, Fugir, UsarItem }
```

## Como executar

```bash
dotnet run
```

## Recursos implementados

- [x] Combate por turnos (Atacar / Defender / Fugir / Usar Item)
- [x] Sistema de níveis e experiência, com suporte a múltiplos level-ups de uma vez
- [x] Curva de XP exponencial
- [x] Múltiplos inimigos em sequência, com dano acumulado entre batalhas (progressão de risco)
- [x] Inventário e poções de cura, com limite de vida máxima
- [x] Correção: pontos de vida não ficam mais negativos ao receber dano fatal
- [x] Correção: mensagem de ganho de XP duplicada removida
- [x] Correção: mensagem de derrota duplicada removida

## Roadmap (próximas fases)

- [ ] Equipamentos (armas/armaduras) com efeito permanente nos atributos

## Tecnologias

- C#
- .NET (Console App / Top-level statements)
