# genAI-gforth

## history

- did some web search and add some resources, then startet codex 0.104.0 gpt-5.3-codex medium
- asked it to read [thinking forth](docs/thinking-forth-color.pdf) and suggest 3 knowledge storage options
- asked it to create json knowledge cards and schema validation
- asked to create more cards

## create a todomvc

- implementation file: `todomvc.fs`
- test file: `todomvc_test.fs`

run tests:

```bash
gforth todomvc_test.fs
```

load and use manually:

```bash
gforth
include todomvc.fs
init-todomvc
s" buy milk" add-todo .
1 toggle-todo .
.stats
bye
```
