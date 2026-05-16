# Quiz — Bônus B3.3

> Não escreva suas respostas neste arquivo — abra o `journal/quiz-notes.md` e escreva lá.
>
> Tente primeiro o `quiz.md` em inglês. Use este aqui só quando uma palavra te travar.

## 1. O que `git reflog` mostra?

- **a.** Todo commit já feito no repositório, do mais antigo primeiro
- **b.** Os branches de rastreamento remoto e as pontas deles
- **c.** Onde o HEAD esteve recentemente — todo checkout, commit, reset, rebase
- **d.** Uma lista dos arquivos modificados no último commit

## 2. Depois de `git reset --hard HEAD~3`, os três commits sumiram?

- **a.** Sim — `reset --hard` apaga eles permanentemente
- **b.** Não — eles ainda estão no armazenamento do git, recuperáveis pelo reflog até o `gc` rodar
- **c.** Só se você já tiver feito push; localmente eles permanecem
- **d.** Só se você tiver um branch de backup apontando para eles

## 3. Qual destes a rede de segurança do git NÃO consegue recuperar?

- **a.** Um commit que você fez e depois passou por cima com `reset --hard`
- **b.** Um branch que você apagou ontem
- **c.** Mudanças não commitadas apagadas por `git restore .`
- **d.** Commits órfãos por um force-push, encontrados em poucas horas

## 4. O que `git fsck --lost-found` faz?

- **a.** Repara os componentes internos corrompidos do git
- **b.** Acha commits soltos e escreve eles em `.git/lost-found/`
- **c.** Puxa todos os branches do remoto
- **d.** Verifica que a árvore de trabalho corresponde ao HEAD

## 5. O que é "a regra do resgate"?

- **a.** Sempre fazer um branch de backup antes de qualquer comando destrutivo
- **b.** Ler o estado (`git status`, `git log`) antes de rodar um comando que muda o histórico
- **c.** Nunca usar `--force` a não ser que você esteja sozinho no repositório
- **d.** Rodar `git gc` depois de todo resgate, para fazer a limpeza

---

Quando terminar, anote suas respostas e uma frase explicando o porquê no `journal/quiz-notes.md` — no mesmo formato das anotações anteriores. Leve à próxima conversa semanal aquela de que você tiver menos certeza.
