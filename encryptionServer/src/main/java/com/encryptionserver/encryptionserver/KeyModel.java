package com.encryptionserver.encryptionserver;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.springframework.data.annotation.Id;

import java.security.PrivateKey;
import java.security.PublicKey;
import java.time.LocalDateTime;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class KeyModel {
    @Id
    private Integer id;
    private byte[] publicKey;
    private byte[] privateKey;
    private boolean isUsed;
    private LocalDateTime createdAt;

    public KeyModel(PublicKey publicKey, PrivateKey privateKey) {
        this.publicKey = publicKey.getEncoded();
        this.privateKey = privateKey.getEncoded();
    }
}
