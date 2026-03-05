def allowed_types:
  ["word", "pattern", "anti_pattern", "chapter_claim", "example", "test_case"];

def sorted_required_keys:
  [
    "embedding_text",
    "id",
    "links",
    "postconditions",
    "preconditions",
    "source_ref",
    "stack_effect",
    "tokens",
    "type"
  ];

def is_string_array:
  type == "array" and all(.[]; type == "string");

def valid_id:
  type == "string" and test("^[a-z0-9._:-]+$");

def valid_type:
  type == "string" and (allowed_types | index(.) != null);

def valid_tokens:
  type == "array"
  and length >= 1
  and all(.[]; type == "string" and length > 0)
  and ((unique | length) == length);

def valid_stack_effect:
  . == null or type == "string";

def valid_source_ref:
  type == "object"
  and (keys | all(. == "source" or . == "locator" or . == "quote_fragment"))
  and has("source")
  and has("locator")
  and (.source | type == "string")
  and (.locator | type == "string")
  and ((has("quote_fragment") | not) or (.quote_fragment | type == "string"));

def valid_card:
  type == "object"
  and ((keys | sort) == sorted_required_keys)
  and (.id | valid_id)
  and (.type | valid_type)
  and (.tokens | valid_tokens)
  and (.stack_effect | valid_stack_effect)
  and (.preconditions | is_string_array)
  and (.postconditions | is_string_array)
  and (.source_ref | valid_source_ref)
  and (.embedding_text | type == "string" and length > 0)
  and (.links | is_string_array);

$cards | all(.[]; valid_card)
