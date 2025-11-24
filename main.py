from transformers import M2M100ForConditionalGeneration, M2M100Tokenizer

def chunk_text(text):
    sentences = text.split('. ')
    chunks = [sentence.strip() + '.' for sentence in sentences if sentence]
    return chunks

with open('input_text.txt', 'r', encoding='utf-8') as file:
    input_text = file.read()

model = M2M100ForConditionalGeneration.from_pretrained("facebook/m2m100_1.2B")
tokenizer = M2M100Tokenizer.from_pretrained("facebook/m2m100_1.2B")

tokenizer.src_lang = "hr"
text_chunks = chunk_text(input_text)

translated_chunks = []
for chunk in text_chunks:
    encoded_chunk = tokenizer(chunk, return_tensors="pt", padding=True, truncation=True)
    generated_tokens = model.generate(**encoded_chunk, forced_bos_token_id=tokenizer.get_lang_id("en"))
    translated_chunks.append(tokenizer.batch_decode(generated_tokens, skip_special_tokens=True)[0])

translated_text = " ".join(translated_chunks)
print(translated_text)

