# RPG em Terminal (C#)

Um RPG por turnos, rodando inteiramente no console, construído como projeto de estudo de Orientação a Objetos em C#.

## Sobre o projeto

Este projeto foi desenvolvido como parte de um plano de estudos de C# e .NET, com foco em aplicar na prática conceitos de:

- Herança e polimorfismo (classes `Personagem`, `Heroi`, `Inimigo`)
- Classes abstratas (`abstract class Personagem`)
- `enum` para representar ações de combate
- `switch` para tratar as escolhas do jogador
- A classe `Random` para calcular dano variável
- Boas práticas de Clean Code e SOLID aplicadas ao design das classes

## Como funciona

O jogo simula um combate por turnos entre um Herói e um Inimigo:

1. A cada turno, o jogador escolhe uma ação (Atacar, Defender ou Fugir)
2. O dano é calculado de forma aleatória, dentro de uma faixa baseada no atributo de Ataque
3. Se o inimigo sobreviver ao ataque, ele contra-ataca automaticamente
4. O combate continua até um dos dois personagens ser derrotado

## Estrutura das classes

```
Personagem (classe abstrata)
├── Nome, PontosDeVida, Ataque, Defesa
├── EstaVivo()
├── ReceberDano(int dano)
│
├── Heroi : Personagem
│   └── Experiencia
│
└── Inimigo : Personagem
    └── RecompensaXP
```

## Como executar

```bash
dotnet run
```

## Roadmap (próximas fases)

- [ ] Sistema de níveis e experiência
- [ ] Múltiplos inimigos e progressão entre batalhas
- [ ] Sistema de inventário e itens (poções, equipamentos)
- [ ] Opções de Defender e Fugir totalmente funcionais

## Tecnologias

- C#
- .NET (Console App / Top-level statements)
