package com.encryptionserver.encryptionserver;

import org.springframework.stereotype.Service;

import javax.crypto.BadPaddingException;
import javax.crypto.Cipher;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;
import java.security.*;
import java.security.spec.InvalidKeySpecException;
import java.security.spec.PKCS8EncodedKeySpec;

@Service
public class EncryptionService {
    private final KeyService keyService;
    private final KeyPair pair;
    private final Cipher cipher;
    private final KeyModel CURRENT_PAIR;

    public EncryptionService(KeyService keyService) throws NoSuchAlgorithmException, NoSuchPaddingException {
        KeyPairGenerator keyPairGen = KeyPairGenerator.getInstance("RSA");

        keyPairGen.initialize(1024);
        pair = keyPairGen.generateKeyPair();
        cipher = Cipher.getInstance("RSA/ECB/PKCS1Padding");

        this.keyService = keyService;
        this.keyService.clearNotUsedPairs();
        CURRENT_PAIR = this.keyService.savePairToDB(new KeyModel(pair.getPublic(), pair.getPrivate()));
    }

    public DataTransfer encrypt(TextTransfer textTransfer) throws IllegalBlockSizeException, BadPaddingException, InvalidKeyException {
        cipher.init(Cipher.ENCRYPT_MODE, pair.getPublic());
        cipher.update(textTransfer.Text().getBytes());

        var data = new DataTransfer(cipher.doFinal());

        keyService.markAsUsed(CURRENT_PAIR);

        return data;
    }

    public TextTransfer decrypt(DataTransfer dataTransfer) throws InvalidKeyException, IllegalBlockSizeException, NoSuchAlgorithmException, InvalidKeySpecException {
        try {
            cipher.init(Cipher.DECRYPT_MODE, pair.getPrivate());
            return new TextTransfer(new String(cipher.doFinal(dataTransfer.Data())));
        }
        catch (BadPaddingException e) {
            return tryOtherKeys(dataTransfer);
        }
    }

    private TextTransfer tryOtherKeys(DataTransfer dataTransfer) throws InvalidKeyException, IllegalBlockSizeException, NoSuchAlgorithmException, InvalidKeySpecException {
        var keyPairs = keyService.getUsedPairs();
        KeyFactory kf = KeyFactory.getInstance("RSA");

        for (var keyPair : keyPairs) {
            try {
                PKCS8EncodedKeySpec spec = new PKCS8EncodedKeySpec(keyPair.getPrivateKey());
                var privateKey = kf.generatePrivate(spec);

                cipher.init(Cipher.DECRYPT_MODE, privateKey);
                return new TextTransfer(new String(cipher.doFinal(dataTransfer.Data())));
            }
            catch (BadPaddingException ignored) {}
        }

        return new TextTransfer("Can't find decryption key");
    }
}
