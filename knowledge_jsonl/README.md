# JSONL Knowledge Cards

Machine-first storage for Forth knowledge retrieval.

## Files

- `schema.json`: canonical card schema.
- `thinking_forth_cards.jsonl`: starter cards from `docs/thinking-forth-color.pdf`.

## Record model

Each line in `*.jsonl` is one independent card. Required keys:

- `id`: stable unique id.
- `type`: one of `word`, `pattern`, `anti_pattern`, `chapter_claim`, `example`, `test_case`.
- `tokens`: retrieval tags and aliases.
- `stack_effect`: normalized stack signature if relevant, else `null`.
- `preconditions`: conditions that must hold before use.
- `postconditions`: guarantees after execution/use.
- `source_ref`: source pointer.
- `embedding_text`: compact semantic string for embedding/vector search.
- `links`: related card ids or external references.

## Retrieval pipeline

1. Filter by metadata (`type`, `tokens`, `stack_effect`).
2. Rerank candidates with BM25/vector using `embedding_text`.
3. Expand neighborhood via `links`.

## Notes

- Prioritize short atomic cards over long narrative chunks.
- Duplicate no facts across cards unless aliases are required for retrieval.

## Validation

``` 

jq -c . knowledge_jsonl/thinking_forth_cards.jsonl || echo "error"
jq -e . knowledge_jsonl/schema.json  || echo "error"
jq -e -n --slurpfile cards knowledge_jsonl/thinking_forth_cards.jsonl -f knowledge_jsonl/validate_cards.jq && echo "schema-ok" || echo "schema-fail"
```