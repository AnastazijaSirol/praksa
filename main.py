
# (Who is danas?????)
'''from transformers import T5Tokenizer, T5ForConditionalGeneration

tokenizer = T5Tokenizer.from_pretrained("google/flan-t5-base")
model = T5ForConditionalGeneration.from_pretrained("google/flan-t5-base")

with open('input_text.txt', 'r') as file:
    input_text = file.read()

input_ids = tokenizer(input_text, return_tensors="pt").input_ids

outputs = model.generate(input_ids)
print(tokenizer.decode(outputs[0]))'''

#OVO JE BOLJE, ALI DOSTA SPORO
'''from transformers import M2M100ForConditionalGeneration, M2M100Tokenizer

with open('input_text.txt', 'r') as file:
    input_text = file.read()

model = M2M100ForConditionalGeneration.from_pretrained("facebook/m2m100_418M")
tokenizer = M2M100Tokenizer.from_pretrained("facebook/m2m100_418M")

tokenizer.src_lang = "hr"
encoded_en = tokenizer(input_text, return_tensors="pt")
generated_tokens = model.generate(**encoded_en, forced_bos_token_id=tokenizer.get_lang_id("en"))
translated = tokenizer.batch_decode(generated_tokens, skip_special_tokens=True)

print(translated[0])'''

from transformers import M2M100ForConditionalGeneration, M2M100Tokenizer

def chunk_text(text):
    sentences = text.split('. ')
    chunks = [sentence.strip() + '.' for sentence in sentences if sentence]
    return chunks

with open('input_text.txt', 'r', encoding='utf-8') as file:
    input_text = file.read()

model = M2M100ForConditionalGeneration.from_pretrained("facebook/m2m100_418M")
tokenizer = M2M100Tokenizer.from_pretrained("facebook/m2m100_418M")

tokenizer.src_lang = "hr"
text_chunks = chunk_text(input_text)

translated_chunks = []
for chunk in text_chunks:
    encoded_chunk = tokenizer(chunk, return_tensors="pt", padding=True, truncation=True)
    generated_tokens = model.generate(**encoded_chunk, forced_bos_token_id=tokenizer.get_lang_id("en"))
    translated_chunks.append(tokenizer.batch_decode(generated_tokens, skip_special_tokens=True)[0])

translated_text = " ".join(translated_chunks)
print(translated_text)

