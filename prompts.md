# Uso de IA no serviço de pontos

A GS permite IA como copiloto, desde que os prompts fiquem registrados e a gente saiba explicar cada linha. Cada entrada: data, quem usou, ferramenta, o que foi pedido e o que foi aproveitado.

## 15/11/2026 · Arthur · Claude Code

- **Pedido:** conferir se o serviço segue os conceitos dos materiais das aulas de SOA (arquitetura em camadas, acoplamento, contratos REST, status codes, tratamento de erros, resiliência, testes, JWT/OAuth) e os requisitos da GS, ajustando o que fosse pequeno.
- **Aproveitado:** a camada de repositório (`IRepositorioPontos`), o 409 para ponto repetido, o contrato único de erro (`timestamp`, `status`, `error`, `message`, `path`), a seção do README que explica a contribuição para cada ODS e a tabela de conceitos com os trade-offs. Autenticação com JWT, retry e versionamento ficaram de fora por decisão nossa, com o motivo registrado no README.

## 15/11/2026 · Arthur · Claude Code

- **Pedido:**Avaliar se valia separar uma camada Application.
- **Aproveitado:** projeto `LuzVital.Pontos.Application`, para onde foram os serviços que antes ficavam dentro de `Infrastructure`.

## 15/11/2026 · Arthur · Claude Code

- **Pedido:** encurtar o README e facilitar a execução para quem não tem o .NET instalado.
- **Aproveitado:** o README reescrito em menos da metade do tamanho, o `Dockerfile` e o `docker-compose.yml` (testados: `docker compose up --build` sobe a API na porta 5132) e o `Directory.Build.props`, que deixa o projeto rodar em versões do .NET mais novas que a 8.
