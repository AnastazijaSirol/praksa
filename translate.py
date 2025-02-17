from transformers import M2M100ForConditionalGeneration, M2M100Tokenizer
import sys
import xml.etree.ElementTree as ET

model = M2M100ForConditionalGeneration.from_pretrained("facebook/m2m100_1.2B")
tokenizer = M2M100Tokenizer.from_pretrained("facebook/m2m100_1.2B")

def translate_text(input_text, src_lang, target_lang):
    tokenizer.src_lang = src_lang
    encoded = tokenizer(input_text, return_tensors="pt")
    generated_tokens = model.generate(**encoded, forced_bos_token_id=tokenizer.get_lang_id(target_lang))
    translated = tokenizer.batch_decode(generated_tokens, skip_special_tokens=True)
    return translated[0]

def translate_resx(file_path, src_lang, target_lang):
    tree = ET.parse(file_path)
    root = tree.getroot()

    for data in root.findall("data"):
        value_element = data.find("value")
        if value_element is not None and value_element.text:
            translated_text = translate_text(value_element.text, src_lang, target_lang)
            value_element.text = translated_text

    return ET.tostring(root, encoding="unicode")

if __name__ == "__main__":
    file_path = sys.argv[1]
    src_lang = sys.argv[2]
    target_lang = sys.argv[3]

    translated_xml = translate_resx(file_path, src_lang, target_lang)
    print(translated_xml)
