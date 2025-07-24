package wharris.SWORD100ServiceAPI;

import java.time.LocalDate;
import java.util.UUID;
import java.util.List;
import java.util.NoSuchElementException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/sword")
public class SwordRestController {

    // if you want an example of making intra-microservice calls via eureka client @Autowired here, go see the student2 code
    @Autowired
    private SwordRepository swordsRepo;

    @GetMapping(path = "")
    @ResponseStatus(code = HttpStatus.OK)
    public List<Sword> findAllBooks() {
        return swordsRepo.findAll();
    }

    @GetMapping(path = "/test")
    @ResponseStatus(code = HttpStatus.OK)
    public String Test() {
        return "hello from sword service api";
    }

    @GetMapping(path = "/search/{searchText}")
    @ResponseStatus(code = HttpStatus.OK)
    public List<Sword> searchItems(@PathVariable(required = true) String searchText) {
        return swordsRepo.findByNameContainingOrDescriptionContaining(searchText, searchText);
    }

    @GetMapping(path = "/{swordUUID}")
    @ResponseStatus(code = HttpStatus.OK)
    public Sword getSword(@PathVariable UUID swordUUID) {
        return swordsRepo.findById(swordUUID).orElseThrow(() -> new NoSuchElementException());
    }

    @PostMapping(path = "")
    @ResponseStatus(code = HttpStatus.CREATED)
    public void createBook(@RequestBody Sword sword) {
        sword.setSwordGuid(UUID.randomUUID());
        sword.setForgedDate(LocalDate.now());
        swordsRepo.save(sword);
    }

    @PostMapping(path = "/addswords")
    @ResponseStatus(code = HttpStatus.CREATED)
    public void createSwords(@RequestBody List<Sword> swords) {

        for (Sword sword : swords) {
            sword.setSwordGuid(UUID.randomUUID());
            sword.setForgedDate(LocalDate.now());
            swordsRepo.save(sword);
        }
    }

    @PutMapping(path = "/{swordUUID}")
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void updateSword(@PathVariable(required = true) UUID swordUUID, @RequestBody Sword sword) {

        if (!sword.getSwordGuid().equals(swordUUID)) {
            throw new RuntimeException(
                    String.format("Path itemId %s did not match body itemId %s",swordUUID, sword.getSwordGuid()));
        }

        swordsRepo.save(sword);
    }

    @DeleteMapping(path = "/{swordUUID}")
    @ResponseStatus(HttpStatus.OK)
    public void DeleteItem(@PathVariable(required = true) UUID swordUUID) {

        swordsRepo.deleteById(swordUUID);
    }

    private static int createRandomIntBetween(int start, int end) {
        return start + (int) Math.round(Math.random() * (end - start));
    }

    private static LocalDate createRandomDate(int startYear, int endYear) {
        int day = createRandomIntBetween(1, 28);
        int month = createRandomIntBetween(1, 12);
        int year = createRandomIntBetween(startYear, endYear);
        return LocalDate.of(year, month, day);
    }
}

