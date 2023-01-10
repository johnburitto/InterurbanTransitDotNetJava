package com.encryptionserver.encryptionserver;

import org.springframework.web.bind.annotation.*;

import javax.crypto.BadPaddingException;
import javax.crypto.IllegalBlockSizeException;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.security.spec.InvalidKeySpecException;

@RestController
@RequestMapping("")
public class EncryptionController {
    public final EncryptionService service;
    public final KeyService keyService;

    public EncryptionController(EncryptionService service, KeyService keyService) {
        this.service = service;
        this.keyService = keyService;
    }

    @GetMapping("/")
    public String hello() {
        return "Hello, i'm encryption server!";
    }

    @PostMapping("/encrypt")
    public DataTransfer encrypt(@RequestBody TextTransfer textTransfer) throws IllegalBlockSizeException, BadPaddingException, InvalidKeyException {
        return service.encrypt(textTransfer);
    }

    @PostMapping("/decrypt")
    public TextTransfer decrypt(@RequestBody DataTransfer dataTransfer) throws IllegalBlockSizeException, InvalidKeyException, NoSuchAlgorithmException, InvalidKeySpecException {
        return service.decrypt(dataTransfer);
    }
}
