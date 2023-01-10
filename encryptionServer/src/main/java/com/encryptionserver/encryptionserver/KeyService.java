package com.encryptionserver.encryptionserver;

import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.List;

@Service
public class KeyService {
    private final KeyRepository repository;

    public KeyService(KeyRepository repository) {
        this.repository = repository;
    }

    public KeyModel savePairToDB(KeyModel model) {
        model.setId(generateNextId());
        model.setCreatedAt(LocalDateTime.now());

        return repository.save(model);
    }

    private Integer generateNextId() {
        return repository.findAll()
                .stream()
                .mapToInt(KeyModel::getId)
                .max()
                .orElse(0) + 1;
    }

    public List<KeyModel> getUsedPairs() {
        return repository.queryGetUsedKeys();
    }

    public void markAsUsed(KeyModel model) {
        if (!model.isUsed()) {
            model.setUsed(true);

            repository.save(model);
        }
    }

    public void clearNotUsedPairs() {
        repository.deleteAll(repository.queryGetNotUsedKeys());
    }
}
