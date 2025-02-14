from transformers import M2M100ForConditionalGeneration, M2M100Tokenizer
import sys

def translate_text(input_text, src_lang, target_lang):
    model = M2M100ForConditionalGeneration.from_pretrained("facebook/m2m100_1.2B")
    tokenizer = M2M100Tokenizer.from_pretrained("facebook/m2m100_1.2B")
    
    tokenizer.src_lang = src_lang
    encoded = tokenizer(input_text, return_tensors="pt")
    generated_tokens = model.generate(**encoded, forced_bos_token_id=tokenizer.get_lang_id(target_lang))
    translated = tokenizer.batch_decode(generated_tokens, skip_special_tokens=True)
    
    return translated[0]

if __name__ == "__main__":
    input_text = sys.argv[1]  
    src_lang = sys.argv[2]    
    target_lang = sys.argv[3] 

    translated_text = translate_text(input_text, src_lang, target_lang)
    print(translated_text)
